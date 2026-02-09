using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public
    int left_dedo_id = 0;
    public
    int right_dedo_id = 0;

    GameObject winAnim, roundAnim;

    FingerControl fingerControl;

    LivesDeactivator livesDeactivator;
    
    EventInstance eventMusic, crowdEffect, eventMusicSelection, crowdTittle, countSound;

    [SerializeField]
    int totalRounds;

    private int round = 0, leftWins = 0;

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
        crowdTittle = RuntimeManager.CreateInstance("event:/Crowd Title");
        countSound = RuntimeManager.CreateInstance("event:/Cuenta UI");

        crowdTittle.start();

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Change_SceneAsync_name(string name)
    {
        //para casos que cargar la escena pueda ser muy lento
        SceneManager.LoadScene(name);
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
        }
        else
        {
            eventMusicSelection.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            crowdEffect.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            crowdTittle.start();

        }
    }
    // Add your game mananger members here
    public void StartCombatMusic()
    {
        crowdTittle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

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
        if (round + 1 >= totalRounds || (!fingerControl.LeftLoose() && leftWins + 1 > totalRounds / 2) || (fingerControl.LeftLoose() && round + 1 - leftWins > totalRounds / 2))
            winAnim.SetActive(true);
        else
            roundAnim.SetActive(true);
    }

    public void StartMusicCount()
    {
        eventMusic.setParameterByNameWithLabel("Parameter", "Sumision");
        crowdEffect.setParameterByNameWithLabel("Parameter", "Sumision");
    }

    public void StartCountUI()
    {
        countSound.start();
    }

    public void StopCount()
    {
        eventMusic.setParameterByNameWithLabel("Parameter", "Play");
        crowdEffect.setParameterByNameWithLabel("Parameter", "Play");
        countSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        winAnim.SetActive(false);
        roundAnim.SetActive(false);
    }

    public void Victory()
    {
        eventMusic.setParameterByNameWithLabel("Parameter", "Win");
        crowdEffect.setParameterByNameWithLabel("Parameter", "Win");

        round++;
        if (!fingerControl.LeftLoose()) leftWins++;
        fingerControl.enabled = false;
        livesDeactivator.DeactivateLives();
    }

    public void StartRound()
    {
        fingerControl.enabled = true;
    }

    public void SetWinAnim(GameObject anim, bool win)
    {
        if(win)
            winAnim = anim;
        else
            roundAnim = anim;
    }

    public void SetFC(FingerControl fc)
    {
        fingerControl = fc;
    }

    public void IncreeseRound()
    {        
        if(round >= totalRounds || leftWins > totalRounds / 2 || round - leftWins > totalRounds / 2)
        {
            round = 0;
            leftWins = 0;
            Change_SceneAsync_name("CharacterSelection");        
        } 
        else        
            Change_SceneAsync_name("Final");        
    }

    public int GetRound() { return round; }
    public int GetLeftWins() { return leftWins; }
    public int GetRightWins() { return round - leftWins; }

    public void SetLivesDeactivator(LivesDeactivator live) { livesDeactivator = live; }
}