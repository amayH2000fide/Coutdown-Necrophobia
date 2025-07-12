using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private SpawnZombieScript spawnZombie;
    private PlayerStatController playerStats;

    public bool isPaused = false;
    public int maxZombiesPerHorde = 10;  
    public float spawnDuration = 60f;   
    public float restDuration = 20f;     
    public int totalHordes = 10;         
    private float gameTimeElapsed = 0f;
    private float totalGameDurationSeconds;
    public int totalGameDurationMinutes = 20;
    public bool isPlayerAlive = true;


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

        spawnZombie = FindObjectOfType<SpawnZombieScript>();
        if (spawnZombie == null)
        {
            Debug.LogWarning("SpawnZombieScript not found in the scene!");
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
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;

        if (isPaused)
            UIOverlayManager.Instance.ShowOverlay("MenuPausa");
        else
            UIOverlayManager.Instance.HideOverlay("MenuPausa");
    }

    public void GameOver()
    {
        UIOverlayManager.Instance.HideAllOverlays();
        SceneLoader.Instance.LoadScene("GameOverScreen");
    }

    private IEnumerator SpawnHordesRoutine()
    {
        for (int i = 0; i < totalHordes; i++)
        {
            float spawnInterval = spawnDuration / maxZombiesPerHorde;

            int spawned = 0;
            while (spawned < maxZombiesPerHorde)
            {
                spawnZombie.SpawnZombie(maxZombiesPerHorde);
                spawned++;

                yield return new WaitForSeconds(spawnInterval);
            }

            // Rest period
            yield return new WaitForSeconds(restDuration);
        }
    }

    private IEnumerator GameLoop()
    {
        while (gameTimeElapsed < totalGameDurationSeconds)
        {
            // Spawn a horde
            yield return StartCoroutine(SpawnHordesRoutine());

            // Rest after horde
            yield return new WaitForSeconds(restDuration);

            gameTimeElapsed += spawnDuration + restDuration;
        }


        if (totalGameDurationSeconds <= 0 && isPlayerAlive)
        {
            GameWon();
        }

        // Game Over or End logic here
        Debug.Log("Game over! 20 minutes elapsed.");
    }


    private void Start()
    {
        StartGame();
        playerStats = FindObjectOfType<PlayerStatController>();

        if (playerStats != null)
        {
            playerStats.OnHealthchanged += CheckPlayerHealth;
        }
        else
        {
            Debug.LogWarning("PlayerStatController not found.");
        }
    }

    public void StartGame()
    {
        totalGameDurationSeconds = totalGameDurationMinutes * 60f;
        gameTimeElapsed = 0f;

        StartCoroutine(GameLoop());
    }

    private void CheckPlayerHealth(int health)
    {
        if (!isPlayerAlive) return;

        int currentHealth = playerStats.GetStat(PlayerStatController.StatType.health);

        if (currentHealth <= 0)
        {
            isPlayerAlive = false;
            PlayerDied();
        }
    }

    private void PlayerDied()
    {
        Time.timeScale = 0f;
        UIOverlayManager.Instance.ShowOverlay("GameOver");
    }

    private void GameWon()
    {
        Time.timeScale = 0f;
        UIOverlayManager.Instance.ShowOverlay("GameWon");
    }

}
