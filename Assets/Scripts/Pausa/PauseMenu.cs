using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string overlaySceneName = "MenuPausa"; 
    [SerializeField] private UIOverlayManager uiOverlayManager;
    public GameObject pauseCanvas;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void Resume()
    {
        Debug.Log("Resume button clicked");

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager.Instance is null!");
            return;
        }

        GameManager.Instance.TogglePause();
    }

    public void LoadMainMenu()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
