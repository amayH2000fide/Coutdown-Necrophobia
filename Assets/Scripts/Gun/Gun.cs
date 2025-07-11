using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    private PlayerStatController playerStats => PlayerStatController.Instance;
    public UnityEvent OnGunShoot;
    public float DamageMultiplier;
    [SerializeField]  private int CurrentDamage;

    public float fireCooldown;
    [SerializeField] private float CurrentCooldown;

    public float reloadTime;
    private float CurrentReload;
    private bool isReloading = false;

    public bool Automatic;
    public bool infiniteAmmo;

    public int maxAmmo;
    public int currentAmmo;

    // balas
    public Transform raycastOrigin;
    public float raycastRange;
    public LayerMask hitMask;

    // granade launcher settings 
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float launchForce;


    public event Action<int> OnAmmoChanged;
    public event Action<int> maxAmmoChanged;

    void Start()
    {
        CurrentCooldown = fireCooldown;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) return;

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }


        if(CurrentCooldown <= 0)
        {
            CurrentCooldown = 0;
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

        if (!infiniteAmmo)
        {
            currentAmmo--;
            Debug.Log("Ammo left: " + currentAmmo);
        }

        if (projectilePrefab != null && spawnPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(spawnPoint.forward * launchForce, ForceMode.Impulse);
            }

        }

        RaycastShoot();
        Debug.Log("shoot!!");
        OnGunShoot?.Invoke();
        CurrentCooldown = fireCooldown;
    }

    public void Reload()
    {
        if (!isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload complete");
    }

    public float GetFinalDamage()
    {
        if (playerStats == null) return 0f;
        CurrentDamage = playerStats.GetStat(PlayerStatController.StatType.damage);
        Debug.Log(CurrentDamage * DamageMultiplier);
        return CurrentDamage * DamageMultiplier;
    }

    public virtual void RaycastShoot()
    {
        if (raycastOrigin == null || playerStats == null) return;
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastRange, hitMask))
        {
            float finalDamage = GetFinalDamage();

            EnemyTestScript enemy = hit.collider.GetComponent<EnemyTestScript>();

            if (enemy != null)
            {
                enemy.TakeDamage(Mathf.RoundToInt(finalDamage));
                Debug.Log("Hit test enemy: " + hit.collider.name);
            }
            else
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
            }
        }
    }
}
