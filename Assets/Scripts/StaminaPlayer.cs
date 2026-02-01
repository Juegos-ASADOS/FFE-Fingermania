using TMPro;
using UnityEngine;

public class StaminaPlayer : MonoBehaviour
{
    // QUITA
    // Te golpean (mucha) x
    // Golpeas (menos, la mitad yoquese) x
    // Moverte (minimo) x
    
    // SE ACABA
    // En 0, tiramos la bola (el head), empieza eltembleke (va subiendo cuando te mueves y cuando llegue a x se pone dura de nuevo)

    // ESTAS TIRAO
    // Si te aplastan, eltembleke recupera menos
    // Empieza la cuenta, si no has recuperao suficiente pa el final cagaste

    [SerializeField]
    public float stamina = 100f;
    float maxStamina;

    [SerializeField]
    float staminaToRecover;

    [SerializeField]
    TextMeshProUGUI staminaTextS;

    [SerializeField]
    public bool isLeft;

    int timesFallen;
    bool recovering;

    private FingerControl fingerCtrl;
    private ErTembleke ertbk;

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
        if (recovering && stamina > staminaToRecover * timesFallen)
        {
            recovering = false;
            ertbk.enabled = false;
            stamina = maxStamina - staminaToRecover * timesFallen;
            fingerCtrl.SwitchMovement(this, false);
            // Llama al manager de la cuenta pa pararla
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
    }

    public void SetFingerControler(FingerControl fg)
    {
        fingerCtrl = fg;
    }

    private void Update()
    {
        //staminaText.text = stamina.ToString() + " - " + timesFallen.ToString();
    }

    public void SetDifficultyTembleke(int difficulty)
    {
        ertbk.SetDifficultyTembleke(difficulty);
    }
}
