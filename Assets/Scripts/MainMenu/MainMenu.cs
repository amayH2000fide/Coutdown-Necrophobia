using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Nombre exacto de la escena de juego")]
    [SerializeField] private string gameSceneName = "REEMPLAZAR_CON_NIVEL";   // cámbiar al nivel 

    // Botón NEW GAME
    public void PlayGame()
    {
        
        Time.timeScale = 1f;             
        SceneManager.LoadScene(gameSceneName);
    }

    // Botón QUIT
    public void QuitGame()
    {
        Application.Quit();              
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }
}
