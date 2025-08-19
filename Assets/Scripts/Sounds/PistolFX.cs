using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PistolFX : MonoBehaviour
{
    public AudioClip shootSound;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0f; 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (shootSound != null)
                source.PlayOneShot(shootSound);
        }
    }
}