using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        UIOverlayManager.Instance.ToggleOverlay("PauseMenuUI");
    }

    public void StartGame()
    {
        SceneLoader.Instance.LoadScene("Level1");
        UIOverlayManager.Instance.ShowOverlay("HUD_UI");
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }

    public void GameOver()
    {
        UIOverlayManager.Instance.HideAllOverlays();
        SceneLoader.Instance.LoadScene("GameOverScreen");
    }
}
