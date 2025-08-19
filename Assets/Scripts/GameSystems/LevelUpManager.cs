using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance { get; private set; }
    public GameObject levelUpPanel;
    public Transform optionContainer;
    public Button optionPrefab;
    public GameObject crosshair;
    
    public PlayerStatController playerStats;
    public MovementController playerMovement;
    public GunSystem gunSystem;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowLevelUp()
    {
        levelUpPanel.SetActive(true);
        playerMovement.canMove = false; 
        crosshair.SetActive(false);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (Transform child in optionContainer)
            Destroy(child.gameObject);

        List<System.Action> upgradeActions = new List<System.Action>();
        List<string> upgradeTexts = new List<string>();

        List<GunSystem.GunData> locked = gunSystem.GetLockedGuns();
        List<GunSystem.GunData> unlocked = gunSystem.GetUnlockedGuns();

        if (locked.Count > 0)
        {
            GunSystem.GunData w = locked[Random.Range(0, locked.Count)];
            upgradeActions.Add(() => UnlockWeapon(w));
            upgradeTexts.Add($"Unlock {w.gunObject}");
        }

        if (unlocked.Count > 0)
        {
            GunSystem.GunData w = unlocked[Random.Range(0, unlocked.Count)];
            upgradeActions.Add(() => UpgradeWeapon(w));
            upgradeTexts.Add($"Upgrade {w.gunObject}");
        }

        List<PlayerStatController.StatType> statChoices = GetRandomStats(3);
        foreach (var stat in statChoices)
        {
            if (upgradeActions.Count >= 3) break;
            PlayerStatController.StatType s = stat;
            upgradeActions.Add(() => UpgradeStat(s));
            upgradeTexts.Add($"Increase {s}");
        }

        while (upgradeActions.Count < 3)
        {
            PlayerStatController.StatType s = (PlayerStatController.StatType)Random.Range(0,
                System.Enum.GetValues(typeof(PlayerStatController.StatType)).Length);
            upgradeActions.Add(() => UpgradeStat(s));
            upgradeTexts.Add($"Increase {s}");
        }

        for (int i = 0; i < upgradeActions.Count && i < 3; i++)
        {
            var btn = Instantiate(optionPrefab, optionContainer);

            var tmpText = btn.GetComponentInChildren<TMP_Text>();
            if (tmpText != null)
                tmpText.text = upgradeTexts[i];

            int idx = i;
            btn.onClick.AddListener(() => SelectUpgrade(upgradeActions[idx]));
        }
    }

    private void SelectUpgrade(System.Action upgradeAction)
    {
        upgradeAction?.Invoke();

        ClosePanel();

        if (playerMovement != null)
            playerMovement.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);
        Time.timeScale = 1f;
    }

    private void ClosePanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void UnlockWeapon(GunSystem.GunData gun)
    {
        int index = gunSystem.guns.IndexOf(gun);
        if (index >= 0)
            gunSystem.UnlockGun(index);
        Debug.Log($"Unlocked weapon: {gun.gunObject.name}");
    }

    private void UpgradeWeapon(GunSystem.GunData gun)
    {
        int index = gunSystem.guns.IndexOf(gun); // get the index of this GunData
        if (index >= 0)
            gunSystem.UpgradeGun(index); // call the existing method
        Debug.Log($"Upgraded weapon: {gun.gunObject.name}");
    }

    private void UpgradeStat(PlayerStatController.StatType stat)
    {
        playerStats.addstats(stat);
        Debug.Log($"Increased {stat}");
    }

    private List<PlayerStatController.StatType> GetRandomStats(int count)
    {
        List<PlayerStatController.StatType> allStats = new List<PlayerStatController.StatType>(
            (PlayerStatController.StatType[])System.Enum.GetValues(typeof(PlayerStatController.StatType)));

        List<PlayerStatController.StatType> selected = new List<PlayerStatController.StatType>();
        while (selected.Count < count && allStats.Count > 0)
        {
            int idx = Random.Range(0, allStats.Count);
            selected.Add(allStats[idx]);
            allStats.RemoveAt(idx);
        }
        return selected;
    }
}
