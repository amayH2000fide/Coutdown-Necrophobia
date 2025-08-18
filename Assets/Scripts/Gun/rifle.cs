using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rifle : Gun
{
    void Start()
    {
        DamageMultiplier = 0.5f;
        infiniteAmmo = false;
        fireCooldown = 0.1f;
        reloadTime = 2f;
        raycastRange = 100f;
        maxAmmo = 60;
        currentAmmo = 60;
    }
}
