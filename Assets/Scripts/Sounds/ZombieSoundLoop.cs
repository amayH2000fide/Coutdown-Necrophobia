using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ZombieSoundLoop : MonoBehaviour
{
    [Header("Sonido del Zombie")]
    public AudioClip zombieLoop;   // arrastra aquí el sonido en loop
    [Range(0f, 1f)] public float volume = 0.7f;

    private AudioSource source;
    private Zombie zombieScript;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
        source.volume = volume;
        source.spatialBlend = 1f;

        zombieScript = GetComponent<Zombie>();
    }

    void Start()
    {
        if (zombieLoop != null)
        {
            source.clip = zombieLoop;
            source.Play(); 
        }
    }

    void Update()
    {
        if (zombieScript != null && zombieScript.estaMuerto)
        {
            if (source.isPlaying)
            {
                source.Stop(); 
            }
        }
    }
}