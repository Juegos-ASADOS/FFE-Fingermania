using System;
using UnityEngine;

public class BlendShape : MonoBehaviour
{
    [Header("References")]
    public Transform fingerBone;

    [Header("References")]
    public SkinnedMeshRenderer skinnedMeshRenderer;

    [Header("Rotation → Blend")]
    public float minAngle = 0f;    // dedo recto
    public float maxAngle = 70f;   // dedo doblado
    public float blendValue;       // 0–100 (resultado)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener rotación local en X
        float angle = fingerBone.localEulerAngles.x;

        // Convertir de 0–360 a -180–180
        if (angle > 180f)
            angle -= 360f;

        // Mapear ángulo a 0–100
        blendValue = Mathf.InverseLerp(minAngle, maxAngle, angle) * 100f;

        // Clamp de seguridad
        blendValue = Mathf.Clamp(blendValue, 0f, 100f);
        skinnedMeshRenderer.SetBlendShapeWeight(0, blendValue);

    }
}
