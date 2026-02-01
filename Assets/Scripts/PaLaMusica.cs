using UnityEngine;

public class PaLaMusica : MonoBehaviour
{
    private void Awake()
    {
        GameManager.instance.SetWinAnim(transform.parent.gameObject);
        transform.parent.gameObject.SetActive(false);
    }

    public void Victory()
    {
        GameManager.instance.Victory();
    }

    public void EndCombat()
    {
        GameManager.instance.Change_SceneAsync_name("CharacterSelection");
    }

    public void Sumission()
    {
        GameManager.instance.StartMusicCount();
    }
}
