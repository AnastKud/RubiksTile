using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("UI")]
    public GameObject startMenu; 
    public GameObject gameUI;  

    private void Start()
    {
        startMenu.SetActive(true);
        gameUI.SetActive(false);
    }

    public void OnStartGame()
    {
        Debug.Log("START GAME");

        startMenu.SetActive(false);
        gameUI.SetActive(true);
    }

    public void OnExitGame()
    {
        Debug.Log("EXIT GAME");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
