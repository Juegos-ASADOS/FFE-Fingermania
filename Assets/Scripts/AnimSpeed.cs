using UnityEngine;

public class AnimSpeed : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Animator>().speed = Random.Range(0.5f, 1.0f);
    }

}
