using FMODUnity;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class lostfocus_ui : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    public GameObject default_button;

    [SerializeField]
    GameObject warningNotController;

    private bool notController = false;

    private void Update()
    {
        if(notController && Gamepad.all.Count > 0)
        {
            notController = false;
            warningNotController.SetActive(false);
        }
        else if(!notController && Gamepad.all.Count <= 0)
        {
            notController = true;
            warningNotController.SetActive(true);
        }
    }

    public void regainFOcus()
    {
        // If nothing is selected, restore focus
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(default_button.gameObject);
        }
    }

    public void start_button()
    {
        if(Gamepad.all.Count > 0)
            GameManager.instance.Change_SceneAsync_name("CharacterSelection");
    }
    public void play_Sound(string ruta)
    {
        RuntimeManager.PlayOneShot(ruta);
    }

}
