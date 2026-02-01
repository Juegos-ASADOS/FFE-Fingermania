using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private float countTime;
    private bool counting;

    public
    int left_dedo_id = 0;
    public
    int right_dedo_id = 0;

    GameObject winAnim;

    FingerControl fingerControl;
    
    EventInstance eventMusic, crowdEffect, eventMusicSelection;

    bool victoryStarted;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        eventMusic = RuntimeManager.CreateInstance("event:/music");
        crowdEffect = RuntimeManager.CreateInstance("event:/Crowd");
        eventMusicSelection = RuntimeManager.CreateInstance("event:/Selection Music");
        instance = this;        
        DontDestroyOnLoad(gameObject);
    }

    public void Change_SceneAsync_name(string name)
    {
        Debug.LogWarning("receurden bloquear input en la carga asyncrona");
        //para casos que cargar la escena pueda ser muy lento
        SceneManager.LoadSceneAsync(name);
        if (name == "Final")
        {   
            eventMusicSelection.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            RuntimeManager.PlayOneShot("event:/Selection End");


            eventMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventMusic.setParameterByNameWithLabel("Parameter", "Play");
            eventMusic.start();
            crowdEffect.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            crowdEffect.setParameterByNameWithLabel("Parameter", "Play");
            crowdEffect.start();
        }
        else if(name == "CharacterSelection")
        {
            eventMusicSelection.start();
            victoryStarted = false;
        }
        else
        {
            eventMusicSelection.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            crowdEffect.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        }
    }
    // Add your game mananger members here
    public void StartCombatMusic()
    {
        eventMusic.setParameterByNameWithLabel("Parameter", "Play");
        eventMusic.start();
        crowdEffect.setParameterByNameWithLabel("Parameter", "Play");
        crowdEffect.start();

        eventMusicSelection.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        RuntimeManager.PlayOneShot("event:/Selection End");
    }

    public void SelectObject(GameObject ob)
    {
        EventSystem.current.SetSelectedGameObject(ob);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartCount()
    {
        counting = true;
        eventMusic.setParameterByNameWithLabel("Parameter", "Sumision");
        crowdEffect.setParameterByNameWithLabel("Parameter", "Sumision");
        winAnim.SetActive(true);
    }

    public void StopCount()
    {
        if (victoryStarted)
            return;

        counting = false;
        countTime = 0;
        eventMusic.setParameterByNameWithLabel("Parameter", "Play");
        crowdEffect.setParameterByNameWithLabel("Parameter", "Play");
        winAnim.SetActive(false);
    }

    public void Victory()
    {
        victoryStarted = true;
        fingerControl.enabled = false;
        eventMusic.setParameterByNameWithLabel("Parameter", "Win");
        crowdEffect.setParameterByNameWithLabel("Parameter", "Win");
    }

    public void SetWinAnim(GameObject anim)
    {
        winAnim = anim;
    }

    public void SetFC(FingerControl fc)
    {
        fingerControl = fc;
    }
}