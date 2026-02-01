using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ErTembleke : MonoBehaviour
{
    Vector2 axes = new Vector2();
    
    [SerializeField]
    Transform fingerHead;

    [SerializeField]
    Vector3 posObjective;
    Vector3 posIni;

    [SerializeField]
    float timeToFall;

    float timeFalling;

    bool falling;

    [SerializeField]
    float moveToRecoverStamina, staminaRecover;

    float movementRecorded;
    StaminaPlayer stPlayer;

    int difficultyMultiplier = 1;


    public void OnFingerMove(CallbackContext context)
    {
        if (!enabled || falling) return;

        movementRecorded += Mathf.Abs((axes - context.ReadValue<Vector2>()).magnitude);
        axes = context.ReadValue<Vector2>();

        if(movementRecorded >= moveToRecoverStamina * difficultyMultiplier)
        {
            movementRecorded = 0;
            stPlayer.recoverStamina(staminaRecover);
        }
    }

    public void Tumbacion(StaminaPlayer stam)
    {
        falling = true;
        posIni = fingerHead.localPosition;
        fingerHead.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        stPlayer = stam;
    }

    // Update is called once per frame
    void Update()
    {
        if (falling) {
            // Animacion de tumbarse
            timeFalling += Time.deltaTime;
            fingerHead.localPosition = Vector3.Lerp(posIni, posObjective, timeFalling/timeToFall);
            //Debug.Log("INI: " + posIni + " OBJETIVO: " + posObjective + " T: " + timeFalling / timeToFall);
            if (timeFalling > timeToFall)
            {
                timeFalling = 0;
                falling = false;
            }
        }
    }

    public void SetDifficultyTembleke(int difficulty)
    {
        difficultyMultiplier = difficulty;
    }
}
