using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class finger_info : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    public Rigidbody Rb;
    [SerializeField]
    public Transform Head;
    [SerializeField]
    [NotNull]
    public StaminaPlayer Stamina;
}
