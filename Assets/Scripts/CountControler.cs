using UnityEngine;

public class CountControler : MonoBehaviour
{
    public void EndRoundCount()
    {
        GameManager.instance.StartRound();
        transform.parent.gameObject.SetActive(false);
    }

    public void SetRoundNumber()
    {
        int round = GameManager.instance.GetRound();
        transform.GetChild(round).gameObject.SetActive(true);
        if(round > 0)
            transform.GetChild(round - 1).gameObject.SetActive(false);
    }
}
