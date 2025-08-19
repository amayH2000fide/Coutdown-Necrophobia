using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{

    [Header("Shooting Effects")]
    public ParticleSystem muzzleFlash;
    public Light flashLight;
    public AudioClip shootSound;

    private AudioSource audioSource;

    private PlayerStatController playerStats => PlayerStatController.Instance;
    [SerializeField] private GunSystem gunManager;
    public bool CanShoot => !isReloading && CurrentCooldown <= 0f;

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

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.clip = shootSound;
    }

    public void Shoot()
    {
        if (!gameObject.activeInHierarchy) return;

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
            Debug.Log("Instantiated projectile: " + projectile.name);

            Rigidbody rb = projectile.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = raycastOrigin.forward;
                rb.AddForce(direction * launchForce, ForceMode.Impulse);
            }

            Grenade grenadeScript = projectile.GetComponent<Grenade>();
            if (grenadeScript != null)
            {
                int finalDamage = Mathf.RoundToInt(GetFinalDamage());
                int gunLevel = gunManager.GetCurrentGunData().level;
                float multiplier = DamageMultiplier;

                grenadeScript.SetDamage(finalDamage, gunLevel, multiplier);
            }
        }

        PlayMuzzleFlash();
        PlayShootSound();

        RaycastShoot();
        Debug.Log("shoot!!");
        OnGunShoot?.Invoke();
        CurrentCooldown = fireCooldown;
    }

    public void TickCooldown()
    {
        if (CurrentCooldown > 0f)
            CurrentCooldown -= Time.deltaTime;

        if (CurrentCooldown < 0f)
            CurrentCooldown = 0f;
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
        if (playerStats == null || gunManager == null) return 0f;

        CurrentDamage = playerStats.GetStat(PlayerStatController.StatType.damage);

        GunSystem.GunData gunData = gunManager.GetCurrentGunData();
        int gunLevel = gunData != null ? gunData.level : 1;

        float gunLevelMultiplier = 1f + 0.3f * (gunLevel - 1);

        float finalDamage = CurrentDamage * DamageMultiplier * gunLevelMultiplier;
        Debug.Log($"Gun Level: {gunLevel}, Base Damage: {CurrentDamage}, Final Damage: {finalDamage}");
        return finalDamage;
    }

    public virtual void RaycastShoot()
    {
        if (raycastOrigin == null || playerStats == null) return;
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastRange, hitMask))
        {
            float finalDamage = GetFinalDamage();

            Zombie enemy = hit.collider.GetComponentInParent<Zombie>();

            if (enemy != null)
            {
                enemy.RecibirDano(Mathf.RoundToInt(finalDamage));
                Debug.Log("Hit test enemy: " + hit.collider.name);
            }
            else
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
            }
        }
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash != null) muzzleFlash.Play();
        if (flashLight != null) StartCoroutine(FlashLight());
    }

    private void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

    private IEnumerator FlashLight()
    {
        flashLight.enabled = true;
        yield return new WaitForSeconds(0.05f);
        flashLight.enabled = false;
    }
}
