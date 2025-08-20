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
        Debug.Log($"=== PICKUP TRIGGER START ===");
        Debug.Log($"Triggered by: {other.name} (Tag: {other.tag})");

        if (!other.CompareTag("Player"))
        {
            Debug.Log("Not player, exiting early");
            return;
        }

        Debug.Log("Player confirmed! Looking for PlayerStatController...");

        PlayerStatController playerStats = other.GetComponent<PlayerStatController>();
        if (playerStats == null)
        {
            Debug.Log("Not found on main object, checking children...");
            playerStats = other.GetComponentInChildren<PlayerStatController>();
        }
        if (playerStats == null)
        {
            Debug.Log("Not found in children, checking parents...");
            playerStats = other.GetComponentInParent<PlayerStatController>();
        }

        if (playerStats == null)
        {
            Debug.LogError("PlayerStatController NOT FOUND anywhere!");
            return;
        }

        Debug.Log($"PlayerStatController found: {playerStats.gameObject.name}");
        Debug.Log($"Processing pickup type: {type}");

        int finalAmount = amount;

        try
        {
            switch (type)
            {
                case PickupType.Health:
                    finalAmount = Mathf.RoundToInt(amount * (1 + 0.2f * playerStats.GetStat(PlayerStatController.StatType.level)));
                    Debug.Log($"Calling RestoreHealth with: {finalAmount}");
                    playerStats.RestoreHealth(finalAmount);
                    break;

                case PickupType.Ammo:
                    Debug.Log("Processing ammo pickup...");
                    GunSystem gunSystem = other.GetComponentInChildren<GunSystem>();
                    if (gunSystem != null)
                    {
                        GameObject currentGunObj = gunSystem.GetCurrentGun();
                        if (currentGunObj != null)
                        {
                            Gun currentGun = currentGunObj.GetComponent<Gun>();
                            if (currentGun != null)
                            {
                                currentGun.currentAmmo = Mathf.Min(currentGun.currentAmmo + amount, currentGun.maxAmmo);
                                Debug.Log($"Ammo set to: {currentGun.currentAmmo}/{currentGun.maxAmmo}");
                            }
                        }
                    }
                    break;

                case PickupType.Experience:
                    finalAmount = Mathf.RoundToInt(amount * (1 + 0.3f * playerStats.GetStat(PlayerStatController.StatType.level)));
                    Debug.Log($"Calling AddExperience with: {finalAmount}");
                    playerStats.AddExperience(finalAmount);
                    break;
            }

            Debug.Log("Pickup processing completed successfully!");
            Debug.Log("About to destroy pickup...");

            // Test if the object is valid before destroying
            if (gameObject != null)
            {
                Destroy(gameObject);
                Debug.Log("Destroy command sent!");
            }
            else
            {
                Debug.LogError("GameObject is already null!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Exception during pickup processing: {e.Message}");
            Debug.LogError($"Stack trace: {e.StackTrace}");
        }

        Debug.Log($"=== PICKUP TRIGGER END ===");
    }

    // Alternative: Also check for collisions in case trigger isn't working
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision with: {collision.gameObject.name} (Tag: {collision.gameObject.tag})");

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collision detected! Destroying pickup.");
            Destroy(gameObject);
        }
    }
}