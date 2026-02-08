using TMPro;
using UnityEngine;

public class StaminaPlayer : MonoBehaviour
{
    [SerializeField]
    float stamina = 100f;
    float maxStamina;

    [SerializeField]
    float staminaToRecover, staminaIncrementToRecover;

    [SerializeField]
    TextMeshProUGUI staminaTextS;

    public bool isLeft;

    int timesFallen;
    bool recovering;

    private FingerControl fingerCtrl;
    private ErTembleke ertbk;

    GameObject icon, hint;

    private void Start()
    {
        maxStamina = stamina;
        ertbk = GetComponent<ErTembleke>();
    }

    public void loseStamina(float amount)
    {
        if (stamina <= 0) return;

        stamina -= amount;
        if (stamina <= 0)
        {
            stamina = 0;
            emptyStamina();
        }
    }

    public void recoverStamina(float amount)
    {
        stamina = Mathf.Min(stamina + amount, 100f);
        if (recovering && stamina > staminaToRecover + staminaIncrementToRecover * timesFallen)
        {
            recovering = false;
            ertbk.enabled = false;
            stamina = maxStamina - staminaToRecover + staminaIncrementToRecover * timesFallen;
            fingerCtrl.SwitchMovement(this, false);
            icon.SetActive(false);
            hint.SetActive(false);
            // Llama al manager de la cuenta pa pararla
            GameManager.instance.StopCount();
        }
    }

    public void emptyStamina()
    {
        if (fingerCtrl.fingerDown)
        {
            stamina = 5;
            return;
        }

        timesFallen++;
        recovering = true;
        fingerCtrl.SwitchMovement(this, true);

        // ELTEMBLEKE
        ertbk.enabled = true;
        ertbk.Tumbacion(this);
        icon.SetActive(true);
        hint.SetActive(true);
    }

    public void SetFingerControler(FingerControl fg)
    {
        fingerCtrl = fg;
        icon = fingerCtrl.GetIcon(isLeft);
        hint = fingerCtrl.GetHint(isLeft);
    }

    public void SetDifficultyTembleke(int difficulty)
    {
        ertbk.SetDifficultyTembleke(difficulty);
    }

    public bool IsDead()
    {
        return stamina <= 0;
    }
}
