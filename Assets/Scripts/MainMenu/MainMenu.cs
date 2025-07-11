using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Nombre exacto de la escena de juego")]
    [SerializeField] private string gameSceneName = "REEMPLAZAR_CON_NIVEL";   // c�mbiar al nivel 

    // Bot�n NEW GAME
    public void PlayGame()
    {
        
        Time.timeScale = 1f;             
        SceneManager.LoadScene(gameSceneName);
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
