using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;   // arrastra tu Canvas
    [Header("Opcional SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip  gameOverSfx;

    private void Start()
    {
        gameOverCanvas.SetActive(false);   // oculto al arrancar
    }

    public void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;               // congela el juego
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;

        if (sfxSource && gameOverSfx)
            sfxSource.PlayOneShot(gameOverSfx);
    }

    // -------- Botones --------
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");   // usa el nombre exacto de tu escena de menú
    }
}
