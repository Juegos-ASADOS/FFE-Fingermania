Shader "Custom/CellShading"
{
    Properties
{
    [Toggle] _UseFresnel("Use Outline", Float) = 1

    _LineThickness("Line Thickness", Float) = 0.02
    _LineColor("Line Color", Color) = (0,0,0,1)

    _Smoothness("Smoothness", Range(0,1)) = 0.5
    _RimThreshold("Rim Threshold", Range(0,5)) = 1
    _RimPower("Rim Power", Range(0,5)) = 1

    _EdgeDiffuse("Edge Diffuse", Range(0,1)) = 0.1

    _EdgeSpecular("Edge Specular", Range(0,1)) = 0.5
    _EdgeSpecularOffset("Edge Specular Offset", Range(-1,1)) = 0
    _EdgeDistanceAttenuation("Edge Distance Attenuation", Range(0,1)) = 0.5
    _EdgeShadowAttenuation("Edge Shadow Attenuation", Range(0,1)) = 0.5
    _EdgeRim("Edge Rim", Range(0,1)) = 0.5
    _EdgeRimOffset("Edge Rim Offset", Range(0,1)) = 0.2
    _ShadowDarknes("Shadow Darkness", Range(0,1)) = 0.2
    _BaseColor("Base Color", Color) = (1,1,1,1)
    _BaseMap("Base Map", 2D) = "white"
}

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float2 uv          : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;

                float _Smoothness;
                float _RimThreshold;
                float _RimPower;

                float _EdgeDiffuse;
                float _EdgeSpecular;
                float _EdgeSpecularOffset;
                float _EdgeDistanceAttenuation;
                float _EdgeShadowAttenuation;
                float _EdgeRim;
                float _EdgeRimOffset;
                float _ShadowDarknes;
                float _UseFresnel;
            CBUFFER_END

            // ======== TU BLOQUE DE CEL SHADING ========

            struct EdgeConstants
            {
                float edgeDiffuse;
                float edgeSpecular;
                float edgeSpecularOffset;
                float edgeDistanceAttenuation;
                float edgeShadowAttenuation;
                float edgeRim;
                float edgeRimOffset;
                float shadowDarknes;
            };

            struct SurfaceVariables
            {
                float3 normal;
                float3 view;
                float smoothness;
                float shininess;
                float rimThreshold;
                float rimPower;
                EdgeConstants ec;
            };

            float3 CalculateCelShading(Light l, SurfaceVariables s)
            {
                float shadowAtt = smoothstep(0.0, s.ec.edgeShadowAttenuation, l.shadowAttenuation);
                float distAtt   = smoothstep(0.0, s.ec.edgeDistanceAttenuation, l.distanceAttenuation);
                float attenuation = shadowAtt * distAtt;

                float ndotl = saturate(dot(s.normal, l.direction)) * step(0.01, attenuation);

                float diffuseBand;
                if (ndotl > 0.1)      diffuseBand = 1.0;
                else if (ndotl > 0.0) diffuseBand = 0.8;
                else                  diffuseBand = s.ec.shadowDarknes;

                float diffuse = diffuseBand;

                float3 h = SafeNormalize(l.direction + s.view);
                float specular = saturate(dot(s.normal, h));
                specular = pow(specular, s.shininess);
                specular *= diffuse * s.smoothness;
                specular = specular > 0.5 ? 1 : 0;

                float rim = 1 - dot(s.view, s.normal);
                rim = saturate(pow(rim, s.rimThreshold));
                float rimCel = rim > 0.1 ? 1 : 0;
                rim = max(rimCel, rim) * diffuse + (rimCel * 0.05 * (1 - diffuse));
                rim *= _UseFresnel;

                return l.color * (diffuse + max(specular, rim));
            }

            // =========================================

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS  = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS    = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv          = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv).rgb * _BaseColor.rgb;

                EdgeConstants ec;
                ec.edgeDiffuse = _EdgeDiffuse;
                ec.edgeSpecular = _EdgeSpecular;
                ec.edgeSpecularOffset = _EdgeSpecularOffset;
                ec.edgeDistanceAttenuation = _EdgeDistanceAttenuation;
                ec.edgeShadowAttenuation = _EdgeShadowAttenuation;
                ec.edgeRim = _EdgeRim;
                ec.edgeRimOffset = _EdgeRimOffset;
                ec.shadowDarknes = _ShadowDarknes;

                SurfaceVariables surface;
                surface.normal = normalize(IN.normalWS);
                surface.view = SafeNormalize(GetWorldSpaceViewDir(IN.positionWS));
                surface.smoothness = _Smoothness;
                surface.rimThreshold = _RimThreshold;
                surface.rimPower = _RimPower;
                surface.shininess = exp2(10 * _Smoothness + 1);
                surface.ec = ec;

                #if SHADOWS_SCREEN
                    float4 shadowCoord = ComputeScreenPos(TransformWorldToHClip(IN.positionWS));
                #else
                    float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                #endif

                Light mainLight = GetMainLight(shadowCoord);
                float3 lighting = CalculateCelShading(mainLight, surface);

                int lightCount = GetAdditionalLightsCount();
                for (int i = 0; i < lightCount; i++)
                {
                    Light l = GetAdditionalLight(i, IN.positionWS);
                    lighting += CalculateCelShading(l, surface);
                }

                return half4(albedo * lighting, 1);
            }

            ENDHLSL
        }
       Pass
{
    Name "Outline"
    Tags { "LightMode"="SRPDefaultUnlit" }
    Cull Front

    HLSLPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    struct Attributes
    {
        float4 positionOS : POSITION;
        float3 normalOS   : NORMAL;
    };

    struct Varyings
    {
        float4 positionHCS : SV_POSITION;
    };

    CBUFFER_START(UnityPerMaterial)
        float _LineThickness;
        half4 _LineColor;
    CBUFFER_END

    Varyings vert(Attributes IN)
    {
        Varyings OUT;

        float3 normalWS   = TransformObjectToWorldNormal(IN.normalOS);
        float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);

        // Push vertices along normals
        positionWS += normalWS * _LineThickness;

        OUT.positionHCS = TransformWorldToHClip(positionWS);
        return OUT;
    }

    half4 frag(Varyings IN) : SV_Target
    {
        return _LineColor;
    }

    ENDHLSL
}

 
 



    }
}
