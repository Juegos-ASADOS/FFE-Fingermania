using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    float defDuration;
    float timeLeft; 

    [SerializeField]
    float defAmount;
    float amount;

    bool shake = false;
    Vector3 startShakePos;

    public void StartShakeDiceGame(float dur, float amo)
    {
        timeLeft = dur;
        amount = amo;
        startShakePos = transform.position;
        shake = true;
    }

    public void StartShakeShooterGame(float dur, float amo)
    {
        timeLeft = dur;
        amount = amo;
        startShakePos = Vector3.zero;
        shake = true;
    }

    public void StartShake()
    {
        timeLeft = defDuration;
        amount = defAmount;
        startShakePos = transform.position;
        shake = true;
    }

    private void Update()
    {
        if (shake)
        {
            if (timeLeft > 0.0f)
            {
                transform.position += Random.insideUnitSphere * amount;

                timeLeft -= Time.deltaTime;
            }
            else
            {
                shake = false;
                if(startShakePos != Vector3.zero)
                    transform.position = startShakePos;
            }
        }
    }
}
