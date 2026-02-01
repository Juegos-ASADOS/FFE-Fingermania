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

}
