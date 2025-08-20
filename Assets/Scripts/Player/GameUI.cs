using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameUI : MonoBehaviour
{
    [Header("Player Reference")]
    public GameObject player;

    [Header("Health Bar")]
    public Slider healthSlider;
    public TMP_Text healthText;

    [Header("Experience Bar")]
    public Slider experienceSlider;
    public TMP_Text experienceText;
    public TMP_Text levelText;

    [Header("Countdown")]
    public TMP_Text countdownText;
    public float countdownTime = 1200f;
    private float currentTime;

    [Header("Ammo")]
    public TMP_Text ammoText;

    [Header("Gun System Reference")]
    public GunSystem gunSystem;

    private PlayerStatController playerStatController;
    private Gun currentGun;
    private Coroutine countdownCoroutine;

    private void Awake()
    {
        Debug.Log("=== GameUI Awake ===");
        FindPlayerStatController();
    }

    private void FindPlayerStatController()
    {
        if (player != null)
        {
            playerStatController = player.GetComponent<PlayerStatController>();
            if (playerStatController != null)
            {
                Debug.Log(" Found PlayerStatController on assigned player");
                return;
            }
        }

        playerStatController = FindObjectOfType<PlayerStatController>();
        if (playerStatController != null)
        {
            Debug.Log("Found PlayerStatController in scene: " + playerStatController.gameObject.name);
            player = playerStatController.gameObject;
            return;
        }

        GameObject playerByTag = GameObject.FindGameObjectWithTag("Player");
        if (playerByTag != null)
        {
            playerStatController = playerByTag.GetComponent<PlayerStatController>();
            if (playerStatController != null)
            {
                Debug.Log("Found PlayerStatController on player-tagged object");
                player = playerByTag;
                return;
            }
        }

        playerStatController = GetComponentInParent<PlayerStatController>();
        if (playerStatController != null)
        {
            Debug.Log("Found PlayerStatController in parent");
            player = playerStatController.gameObject;
            return;
        }

        Debug.LogError("Could not find PlayerStatController anywhere!");
    }

    private void Start()
    {
        Debug.Log("=== GameUI Start ===");

        StartCoroutine(InitializeAfterDelay());
        currentTime = countdownTime;
        countdownCoroutine = StartCoroutine(CountdownTimer());
    }

    private IEnumerator InitializeAfterDelay()
    {
        yield return new WaitForEndOfFrame();

        InitializeUI();
    }

    private void InitializeUI()
    {
        Debug.Log("Initializing UI...");

        if (playerStatController != null)
        {
            Debug.Log("PlayerStatController found, setting up UI with real values");

            int maxHealth = playerStatController.GetStat(PlayerStatController.StatType.maxHealth);
            int currentHealth = playerStatController.GetStat(PlayerStatController.StatType.health);

            Debug.Log($"Health values - Current: {currentHealth}, Max: {maxHealth}");

            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            healthText.text = $"{currentHealth}/{maxHealth}";

            int currentLevel = playerStatController.GetStat(PlayerStatController.StatType.level);
            int expToNextLevel = playerStatController.ExperienceToLevelUp;
            int currentExp = playerStatController.experience;

            Debug.Log($"Experience values - Level: {currentLevel}, CurrentExp: {currentExp}, NextLevelExp: {expToNextLevel}");

            experienceSlider.minValue = 0;
            experienceSlider.maxValue = expToNextLevel;
            experienceSlider.value = currentExp;
            experienceText.text = $"{currentExp}/{expToNextLevel}";
            levelText.text = $"Level {currentLevel}";

            Debug.Log("UI Initialized successfully");
        }
        else
        {
            Debug.LogError("PlayerStatController is null in InitializeUI!");
            SetupDefaultUI();
        }

        UpdateAmmoUI();
    }

    private void SetupDefaultUI()
    {
        Debug.Log("Setting up default UI values");

        healthSlider.minValue = 0;
        healthSlider.maxValue = 100;
        healthSlider.value = 100;
        healthText.text = "100/100";

        experienceSlider.minValue = 0;
        experienceSlider.maxValue = 100;
        experienceSlider.value = 0;
        experienceText.text = "0/100";
        levelText.text = "Level 1";
    }

    private void OnEnable()
    {
        Debug.Log("GameUI enabled - subscribing to events");

        if (playerStatController != null)
        {
            playerStatController.OnHealthchanged += UpdateHealthUI;
            playerStatController.OnLevelChanged += UpdateLevelUI;
            playerStatController.OnExperienceChanged += UpdateExperienceUI;
            Debug.Log("Subscribed to stat controller events");
        }
        else
        {
            Debug.LogWarning("Cannot subscribe to events: playerStatController is null");
        }
    }


    private void OnDisable()
    {
        Debug.Log("GameUI disabled - unsubscribing from events");

        if (playerStatController != null)
        {
            playerStatController.OnHealthchanged -= UpdateHealthUI;
            playerStatController.OnLevelChanged -= UpdateLevelUI;
            playerStatController.OnExperienceChanged -= UpdateExperienceUI;
            Debug.Log("Unsubscribed from stat controller events");
        }

        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
    }

    private void Update()
    {

        UpdateAmmoUI();

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Manual UI refresh triggered");
            InitializeUI();
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        Debug.Log($"Health UI Update received: {currentHealth}/{maxHealth}");

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
        Debug.Log($"Health UI updated to: {currentHealth}/{maxHealth}");
    }

    private void UpdateLevelUI(int newLevel)
    {
        Debug.Log($"Level UI Update received: {newLevel}");

        levelText.text = $"Level {newLevel}";

        if (playerStatController != null)
        {
            int expToNextLevel = playerStatController.ExperienceToLevelUp;
            int currentExp = playerStatController.experience;

            experienceSlider.maxValue = expToNextLevel;
            experienceSlider.value = currentExp;
            experienceText.text = $"{currentExp}/{expToNextLevel}";

            Debug.Log($"Level updated to: {newLevel}, Exp: {currentExp}/{expToNextLevel}");
        }
    }

    private void UpdateExperienceUI(int newExperience)
    {
        Debug.Log($"Experience UI Update received: {newExperience}");

        if (playerStatController != null)
        {
            int expToNextLevel = playerStatController.ExperienceToLevelUp;
            int currentLevel = playerStatController.GetStat(PlayerStatController.StatType.level);

            experienceSlider.maxValue = expToNextLevel;
            experienceSlider.value = newExperience;
            experienceText.text = $"{newExperience}/{expToNextLevel}";

            levelText.text = $"Level {currentLevel}";

            Debug.Log($"Experience updated to: {newExperience}/{expToNextLevel}");
        }
    }

    private void UpdateAmmoUI()
    {
        if (gunSystem != null)
        {
            GameObject currentGunObject = gunSystem.GetCurrentGun();

            if (currentGunObject != null)
            {
                currentGun = currentGunObject.GetComponent<Gun>();

                if (currentGun != null)
                {
                    ammoText.text = $"{currentGun.currentAmmo}/{currentGun.maxAmmo}";

                    if (currentGun.infiniteAmmo)
                    {
                        ammoText.text = "∞";
                    }

                    if (currentGun.currentAmmo == 0)
                    {
                        ammoText.color = Color.red;
                    }
                    else if (currentGun.currentAmmo <= currentGun.maxAmmo / 4)
                    {
                        ammoText.color = Color.yellow;
                    }
                    else
                    {
                        ammoText.color = Color.white;
                    }
                    return;
                }
            }
        }

        // Fallback: Try to find GunSystem
        if (player != null && gunSystem == null)
        {
            gunSystem = player.GetComponentInChildren<GunSystem>();
        }

        ammoText.text = "No Ammo Info";
        ammoText.color = Color.gray;
    }

    private IEnumerator CountdownTimer()
    {
        while (currentTime > 0)
        {
            // Format time as minutes:seconds
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            countdownText.text = $"{minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        // Countdown finished
        countdownText.text = "00:00";
        Debug.Log("Countdown finished!");
    }

    // Debug method to check current state
    [ContextMenu("Debug UI State")]
    public void DebugUIState()
    {
        Debug.Log("=== UI Debug State ===");
        Debug.Log($"Player: {player != null}");
        Debug.Log($"PlayerStatController: {playerStatController != null}");
        Debug.Log($"Health Slider: {healthSlider.value}/{healthSlider.maxValue}");
        Debug.Log($"Experience Slider: {experienceSlider.value}/{experienceSlider.maxValue}");
        Debug.Log("=== End Debug ===");
    }
}