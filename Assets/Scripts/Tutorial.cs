using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    lostfocus_ui ui;

    public void EndTutorial()
    {
        ui.tutorialEnd = true;
    }
}
