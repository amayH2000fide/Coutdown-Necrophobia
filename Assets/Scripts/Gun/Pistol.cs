using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    void Start()
    {
        DamageMultiplier = 1f;
        infiniteAmmo = true;
        fireCooldown = 0.4f;
        reloadTime = 1.5f;
        raycastRange = 100f;    
    }
}
