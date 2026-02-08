using UnityEngine;

public class CountControler : MonoBehaviour
{
    public void EndRoundCount()
    {
        GameManager.instance.StartRound();
        transform.parent.gameObject.SetActive(false);
    }
}
