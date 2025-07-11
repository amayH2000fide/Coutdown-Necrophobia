using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    private CharacterController controller;
    private PlayerStatController statController;

    public LayerMask groundMask;
    private Vector2 inputMovement;
    private Vector2 inputMouse;       
    private Vector3 moveDirection;
    private float xRotation = 0f;
    public float jumpForce;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;

    public Transform groundCheck;
    public Transform cameraTransform;
    public bool isGrounded;
    public float mouseSensitivity;
   
    public bool isSprinting;
    public bool jumpPressed;

    private Vector3 velocity;
    public Vector3 CurrentVelocity => velocity;
    public float CurrentSpeed => new Vector3(velocity.x, 0f, velocity.z).magnitude;

    public bool IsGrounded()
    {
        return controller.isGrounded;
    }

    public void GetInputs()
    {
        inputMovement.x = Input.GetAxisRaw("Horizontal");
        inputMovement.y = Input.GetAxisRaw("Vertical");
        inputMouse.x = Input.GetAxis("Mouse X");
        inputMouse.y = Input.GetAxis("Mouse Y");
        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        jumpPressed = Input.GetButtonDown("Jump");
    }


    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 1);
    }

    private void GroundCheck()
    {
        Vector3 origin = groundCheck.position;
        isGrounded = Physics.Raycast(origin, Vector3.down, 1 , groundMask);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -1f;
        }
    }



    public void MoveCamera()
    {
        xRotation -= inputMouse.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -30f, 20f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * inputMouse.x * mouseSensitivity);
    }

    public void MoveSelf()
    {
        float acceleration = 5f;
        float gravity = -9.81f;

        float targetSpeed = GetCurrentSpeed();

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Vector3 targetVelocity = moveDirection.normalized * targetSpeed;
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, acceleration * Time.deltaTime);
            velocity.z = Mathf.MoveTowards(velocity.z, targetVelocity.z, acceleration * Time.deltaTime);
        }
        else
        {
            float drag = 10f; // Snappier stopping
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, drag * Time.deltaTime);
            velocity.z = Mathf.MoveTowards(velocity.z, 0f, drag * Time.deltaTime);
        }

        if (isGrounded)
        {
            if (jumpPressed)
            {
                velocity.y = jumpForce;
            }
            else if (velocity.y < 0f)
            {
                velocity.y = -1f; 
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; 
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public float GetCurrentSpeed()
    {
        float speedStat = statController.GetStat(PlayerStatController.StatType.Speed);
        float maxSpeed = statController.GetStat(PlayerStatController.StatType.MaxSpeed);

        if (isSprinting)
        {
            speedStat = Mathf.Min(speedStat * 2f, 100f);
            maxSpeed *= 2f;
        }

        float normalizedSpeed = Mathf.Clamp01(speedStat / 100f);
        float speed = Mathf.Lerp(speedStat, maxSpeed, Mathf.Pow(normalizedSpeed, 2));

        return speed;
    }

    void MoveDirection()
    {
        Vector3 move = transform.right * inputMovement.x + transform.forward * inputMovement.y;
        moveDirection = move.normalized;

    }


    void Start()
    {
        controller = GetComponent<CharacterController>();
        statController = GetComponent<PlayerStatController>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        GroundCheck();
        GetInputs();      
        MoveDirection();   
        MoveCamera();      
        MoveSelf();
    }
}
