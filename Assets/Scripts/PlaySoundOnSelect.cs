using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnSelect : MonoBehaviour, ISelectHandler
{
    public string rutaAudio;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Pasa por encima, o selecciona");
        RuntimeManager.PlayOneShot(rutaAudio);
    }
}
