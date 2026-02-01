using FMODUnity;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class lostfocus_ui : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    public GameObject default_button;

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
        GameManager.instance.Change_SceneAsync_name("CharacterSelection");
    }
    public void play_Sound(string ruta)
    {
        RuntimeManager.PlayOneShot(ruta);
    }

}
