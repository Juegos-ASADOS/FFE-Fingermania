using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class ClickToStart : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)        
            GetComponent<Animator>().SetTrigger("click");        
    }

    public void Transition()
    {
        SceneManager.LoadScene("MainTitle_Fin");
    }
}
