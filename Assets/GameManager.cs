using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private GameManager()
    {
        // initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
        // because the game manager will be created before the objects
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
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
}