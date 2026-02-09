using UnityEngine;
using UnityEngine.UI;

public class LivesDeactivator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.SetLivesDeactivator(this);
        DeactivateLives();
    }

    public void DeactivateLives()
    {
        for (int i = 0; i < GameManager.instance.GetLeftWins(); i++)
            transform.GetChild(0).GetChild(i).GetComponent<RawImage>().color = new Color(69 / 365f, 88 / 365f, 106 / 365f, 50 / 365f);

        for (int i = 0; i < GameManager.instance.GetRightWins(); i++)
            transform.GetChild(1).GetChild(i).GetComponent<RawImage>().color = new Color(69 / 365f, 88 / 365f, 106 / 365f, 50 / 365f);
    }
}
