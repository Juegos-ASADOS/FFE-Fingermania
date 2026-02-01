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
    
    EventInstance eventMusic, crowdEffect;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        eventMusic = RuntimeManager.CreateInstance("event:/music");
        crowdEffect = RuntimeManager.CreateInstance("event:/Crowd");
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
            eventMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventMusic.setParameterByNameWithLabel("Parameter", "Play");
            eventMusic.start();
            crowdEffect.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            crowdEffect.setParameterByNameWithLabel("Parameter", "Play");
            crowdEffect.start();
        }
    }
    // Add your game mananger members here
    public void StartCombatMusic()
    {
        eventMusic.setParameterByNameWithLabel("Parameter", "Play");
        eventMusic.start();
        crowdEffect.setParameterByNameWithLabel("Parameter", "Play");
        crowdEffect.start();
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
        counting = false;
        countTime = 0;
        eventMusic.setParameterByNameWithLabel("Parameter", "Play");
        crowdEffect.setParameterByNameWithLabel("Parameter", "Play");
        winAnim.SetActive(false);
    }

    public void Victory()
    {
        eventMusic.setParameterByNameWithLabel("Parameter", "Win");
        crowdEffect.setParameterByNameWithLabel("Parameter", "Win");
    }

    public void SetWinAnim(GameObject anim)
    {
        winAnim = anim;
    }
}