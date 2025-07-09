using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public UnityEvent OnGunShoot;
    public float fireCooldown;
    public bool Automatic;
    private float CurrentCooldown;
    public int maxAmmo;
    public int currentAmmo;
    public bool infiniteAmmo;
    public GameObject projectilePrefab;
    public Transform spawnPoint;

    void Start()
    {
        CurrentCooldown = fireCooldown;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (Automatic)
        {
            if (Input.GetMouseButton(0) && CurrentCooldown <= 0f)
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && CurrentCooldown <= 0f)
            {
                Shoot();
            }
        }

        CurrentCooldown -= Time.deltaTime;
    }


    public void Shoot()
    {
        if (!infiniteAmmo && currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        OnGunShoot?.Invoke();
        CurrentCooldown = fireCooldown;

        if (!infiniteAmmo)
        {
            currentAmmo--;
            Debug.Log("Ammo left: " + currentAmmo);
        }
    }
}
