using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepLoop : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip footstepLoop;          
    public float volume = 0.8f;

    [Header("Opcional (mejor detección)")]
    public CharacterController controller;  
    public bool requireGrounded = true;    
    public float minSpeed = 0.1f;           

    private AudioSource source;


    private readonly KeyCode[] moveKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;         
        source.spatialBlend = 0f;   
        source.volume = volume;
        if (footstepLoop != null) source.clip = footstepLoop;
    }

    void Update()
    {
        bool holdingMoveKey = false;
        for (int i = 0; i < moveKeys.Length; i++)
        {
            if (Input.GetKey(moveKeys[i]))
            {
                holdingMoveKey = true;
                break;
            }
        }

        bool groundedOK = !requireGrounded || controller == null || controller.isGrounded;

        bool speedOK = true;
        if (controller != null)
            speedOK = controller.velocity.magnitude > minSpeed;

        if (holdingMoveKey && groundedOK && speedOK && footstepLoop != null)
        {
            if (!source.isPlaying) source.Play();
        }
        else
        {
            if (source.isPlaying) source.Stop();
        }
    }
}