using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Nombre exacto de la escena de juego")]
    [SerializeField] private string gameSceneName = "MainScene";

    // Bot�n NEW GAME
    public void PlayGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.isPaused = false;
        SceneManager.LoadScene("MainScene");
        
    }

    // Bot�n QUIT
    public void QuitGame()
    {
        Application.Quit();              
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }
}
