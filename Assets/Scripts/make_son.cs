using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class make_son : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    GameObject ob;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ob.transform.SetParent(transform);
    }

}
