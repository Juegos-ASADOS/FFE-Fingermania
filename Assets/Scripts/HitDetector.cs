using UnityEngine;

public class HitDetector : MonoBehaviour
{

    [SerializeField]
    StaminaPlayer playerStamina;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("he tocado a " + collision.gameObject.name);
        if (collision.gameObject.TryGetComponent<HitDetector>(out HitDetector other))
        {
            Debug.Log("se han pegado!");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<HitDetector>(out HitDetector other))
        {
            Debug.Log("se estan pegado!");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<HitDetector>(out HitDetector other))
        {
            Debug.Log("ya no se quieren!");
        }
    }
}
