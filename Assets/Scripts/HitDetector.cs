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
        isLeft = playerStamina.isLeft;
        fc = GetComponentsInParent<FingerControl>().First();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out HitDetector other) && other.isLeft != this.isLeft)
        {
            if (isLeft)
                fc.ManageFingerColision(transform.position, other.transform.position);
            else
                fc.ManageFingerColision(other.transform.position, transform.position);
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
            fc.ManageFingerColisionExit();
        }
    }
}
