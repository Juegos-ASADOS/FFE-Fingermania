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
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (counting)
            countTime += Time.deltaTime;
        
        if (countTime > 3f)
        {
            SceneManager.LoadScene("CharacterSelection");
            counting = false;
            countTime = 0f;
        }
    }
    public void Change_SceneAsync_name(string name)
    {
        Debug.LogWarning("receurden bloquear input en la carga asyncrona");
       //para casos que cargar la escena pueda ser muy lento
        SceneManager.LoadSceneAsync(name);
    }
    // Add your game mananger members here
    public void Pause(bool paused)
    {
    }

    public void SelectObject(GameObject ob)
    {
        EventSystem.current.SetSelectedGameObject(ob);
    }
    public void ExitGame()
    {

        //TODO maybe playear una animación.
        Application.Quit();
    }

    public void StartCount()
    {
        counting = true;
    }

    public void StopCount()
    {
        counting = false;
        countTime = 0;
    }
}