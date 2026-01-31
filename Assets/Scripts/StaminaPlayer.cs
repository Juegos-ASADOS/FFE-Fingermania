using UnityEngine;

public class StaminaPlayer : MonoBehaviour
{
    [SerializeField]
    public float stamina = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("la toca!" + collision.gameObject.name);

        if (collision.gameObject.TryGetComponent<StaminaPlayer>(out StaminaPlayer other))
        {
            Debug.Log("se han pegado!");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<StaminaPlayer>(out StaminaPlayer other))
        {
            Debug.Log("se estan pegado!");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<StaminaPlayer>(out StaminaPlayer other))
        {
            Debug.Log("ya no se quieren!");
        }
    }

}
