using UnityEngine;

public class RotateFinger : MonoBehaviour
{
    float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(0.15f, 0.25f);       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed, 0);       
    }
}
