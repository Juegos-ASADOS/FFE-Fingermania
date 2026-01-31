#pragma multi_compile _ _ADDITIONAL_LIGHTS
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX
#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

#ifndef LIGHTING_CEL_SHADED_INCLUDED
#define LIGHTING_CEL_SHADED_INCLUDED


#ifndef SHADERGRAPH_PREVIEW

    struct EdgeConstants
    {
        float edgeDiffuse; // Suavizado del borde difuso
        float edgeSpecular; // Posici�n del specular
        float edgeSpecularOffset; // Desplazamiento del specular
        float edgeDistanceAttenuation; // Suavizado por distancia
        float edgeShadowAttenuation; // Suavizado de sombras
        float edgeRim; // Posici�n del rim light
        float edgeRimOffset; // Anchura del rim
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
        float distAtt = smoothstep(0.0, s.ec.edgeDistanceAttenuation, l.distanceAttenuation);
        float attenuation = shadowAtt * distAtt;
    float shadow = attenuation;
    float ndotl = saturate(dot(s.normal, l.direction)) * step(0.01, shadow);
        // 0 = sombra, 1 = luz
    float diffuseBand;

    if (ndotl > 0.1)
        diffuseBand = 1.0;
    else if (ndotl > 0.0)
        diffuseBand = 0.8;
    else
        diffuseBand = s.ec.shadowDarknes;
    float diffuse = diffuseBand ;
        //diffuse = smoothstep(0.0, s.ec.edgeDiffuse, diffuse);

        float3 h = SafeNormalize(l.direction + s.view);
        float specular = saturate(dot(s.normal, h));
        specular = pow(specular, s.shininess);
        specular *= diffuse * s.smoothness;
        specular = specular > 0.5 ? 1 : 0;

        //specular = s.smoothness * smoothstep(
        //    (1 - s.smoothness) * s.ec.edgeSpecular + s.ec.edgeSpecularOffset,
        //    s.ec.edgeSpecular + s.ec.edgeSpecularOffset,
        //    specular
        //);

        float rim = 1 - (dot(s.view, s.normal)); 
        rim = saturate(pow(rim, s.rimThreshold) );
        float  rimCel = rim > 0.1 ? 1 : 0;
        rim = max(rimCel, rim) * diffuse + (rimCel*0.05 * (1 - diffuse));
    
    
    //max(((1 - diffuse) * 0.5 + diffuse) * rim * s.rimPower, rimCel * s.rimPower * diffuse);

        //rim = s.smoothness * smoothstep(
        //    s.ec.edgeRim - 0.5 * s.ec.edgeRimOffset,
        //    s.ec.edgeRim + 0.5 * s.ec.edgeRimOffset,
        //    rim
        //);

    return l.color * (diffuse + max(specular, rim));
}

#endif

void LightingCelShaded_float(
    float Smoothness,
    float RimThreshold,
    float3 Position,
    float3 Normal,
    float3 View,
    float EdgeDiffuse,
    float EdgeSpecular,
    float EdgeSpecularOffset,
    float EdgeDistanceAttenuation,
    float EdgeShadowAttenuation,
    float EdgeRim,
    float EdgeRimOffset,
    float RimPower,
float shadowDarknes,
    out float3 Color)
{
    float3 col = float3(0, 0, 0);
#if defined(SHADERGRAPH_PREVIEW)
    col = float3(1, 0.5, 0.5);
#else
        EdgeConstants ec;
        ec.edgeDiffuse = EdgeDiffuse;
        ec.edgeSpecular = EdgeSpecular;
        ec.edgeSpecularOffset = EdgeSpecularOffset;
        ec.edgeDistanceAttenuation = EdgeDistanceAttenuation;
        ec.edgeShadowAttenuation = EdgeShadowAttenuation;
        ec.edgeRim = EdgeRim;
        ec.edgeRimOffset = EdgeRimOffset;
    ec.shadowDarknes = shadowDarknes;

        SurfaceVariables surface;
        surface.normal = normalize(Normal);
        surface.view = SafeNormalize(View);
        surface.smoothness = Smoothness;
        surface.rimThreshold = RimThreshold;
        surface.rimPower = RimPower;
        surface.shininess = exp2(10 * Smoothness + 1);
        surface.ec = ec;

    #if SHADOWS_SCREEN
            float4 clipPos = TransformWorldToHClip(Position);
            float4 shadowCoord = ComputeScreenPos(clipPos);
    #else
        float4 shadowCoord = TransformWorldToShadowCoord(Position);
    #endif

        Light light = GetMainLight(shadowCoord);
        col = CalculateCelShading(light, surface);

        int pixelLightCount = GetAdditionalLightsCount();
        for (int i = 0; i < pixelLightCount; i++)
        {
        //Color = float3(1.0, 0.0, 0.0);
            light = GetAdditionalLight(i, Position);
            col += CalculateCelShading(light, surface);
        }
#endif
    Color=col;
}

#endif
