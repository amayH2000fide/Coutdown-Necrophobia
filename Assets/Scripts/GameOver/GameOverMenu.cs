using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField ] private GameObject gameOverCanvas;   // arrastra tu Canvas
    [Header("Opcional SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip  gameOverSfx;

    private void Start()
    {
        ShowGameOver();
    }

    public void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;

        if (sfxSource && gameOverSfx)
            sfxSource.PlayOneShot(gameOverSfx);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
