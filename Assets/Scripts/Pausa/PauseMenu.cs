using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;   // Arrastra tu Canvas aquí

    private bool isPaused = false;

    private void Start()
    {
        SetPause(false);                // Asegura que el juego arranca sin pausa
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    // ───── Funciones que llamarás desde los botones ─────
    public void Resume()          => SetPause(false);

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");   // Cambia el nombre si tu escena se llama distinto
    }

    // ───── Lógica interna ─────
    private void TogglePause()    => SetPause(!isPaused);

    private void SetPause(bool value)
    {
        isPaused                = value;
        pauseCanvas.SetActive(value);     // Muestra/oculta el Canvas
        Time.timeScale          = value ? 0f : 1f;   // Congela o reanuda el tiempo
        Cursor.lockState        = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible          = value;
    }
}
