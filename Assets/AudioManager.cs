using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    Bus masterBus;

    void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
    }

    public void SetMasterVolume(float volume)
    {
        // volume va de 0.0f (mute) a 1.0f (normal)
        masterBus.setVolume(volume);
    }
}
