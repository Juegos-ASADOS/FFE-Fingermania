using FMODUnity;
using UnityEngine;

public class RoundSound : MonoBehaviour
{
    public void Bell()
    {
        RuntimeManager.PlayOneShot("event:/Bell");
        GameManager.instance.StartCombatMusic();
    }
}
