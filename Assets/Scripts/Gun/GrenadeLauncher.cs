using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Gun
{
    public GameObject grenadePrefab;
    public Transform launchPoint;

    void Start()
    {
        DamageMultiplier = 3f;
        infiniteAmmo = false;
        fireCooldown = 0.4f;
        reloadTime = 1.5f;
        raycastRange = 100f;
        maxAmmo = 5;
        currentAmmo = 5;
        launchForce = 10;
        projectilePrefab = grenadePrefab;
        spawnPoint = launchPoint;
    }
}
