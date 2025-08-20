using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class GunShootSFX : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip shootClip;          
    public float volume = 1f;

    [Header("Refs")]
    public Gun gun;                      
    private AudioSource source;

    void Awake()
    {
        if (gun == null) gun = GetComponent<Gun>();
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0f; 
        source.volume = volume;
    }

    void OnEnable()
    {
        if (gun != null)
            gun.OnGunShoot.AddListener(OnGunShoot);
    }

    void OnDisable()
    {
        if (gun != null)
            gun.OnGunShoot.RemoveListener(OnGunShoot);
    }

    private void OnGunShoot()
    {
        if (shootClip != null)
            source.PlayOneShot(shootClip);
    }
}
