using System.Linq;
using UnityEngine;

public class HitDetector : MonoBehaviour
{

    [SerializeField]
    StaminaPlayer playerStamina;

    [SerializeField]
    bool isLeft;

    FingerControl fc;
    private void Start()
    {
        fc = GetComponentsInParent<FingerControl>().First();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out HitDetector other))
        {
            fc.ManageFingerColision(collision.contacts[0].point);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out HitDetector other))
        {
            //Debug.Log("se estan pegado!");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out HitDetector other))
        {
            //Debug.Log("ya no se quieren!");
        }
    }
}
