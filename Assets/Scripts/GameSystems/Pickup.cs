using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Health,
    Ammo,
    Experience
}

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int amount = 25;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.name + " | Tag: " + other.tag);

        if (!other.CompareTag("Player")) return;

        PlayerStatController playerStats = other.GetComponentInChildren<PlayerStatController>();
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStatController not found on: " + other.name);
            return;
        }

        int finalAmount = amount;

        switch (type)
        {
            case PickupType.Health:
                finalAmount = Mathf.RoundToInt(amount * (1 + 0.2f * playerStats.GetStat(PlayerStatController.StatType.level)));
                playerStats.RestoreHealth(finalAmount);
                break;

            case PickupType.Ammo:
                Gun currentGun = other.GetComponentInChildren<GunSystem>().GetCurrentGun()?.GetComponent<Gun>();
                if (currentGun != null)
                {
                    currentGun.currentAmmo = currentGun.maxAmmo;
                }
                break;

            case PickupType.Experience:
                finalAmount = Mathf.RoundToInt(amount * (1 + 0.3f * playerStats.GetStat(PlayerStatController.StatType.level)));
                playerStats.AddExperience(finalAmount);
                break;
        }

        Destroy(gameObject);
    }
}
