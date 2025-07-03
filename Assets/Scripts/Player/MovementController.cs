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
    private Vector3 velocity;
    private float xRotation = 0f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    [SerializeField] private float groundDrag = 10f;

    public Transform groundCheck;
    public Transform cameraTransform; 

    public float mouseSensitivity;
    public bool isGrounded;


    public void GetInputs()
    {
        inputMovement.x = Input.GetAxis("Horizontal");
        inputMovement.y = Input.GetAxis("Vertical");
        inputMouse.x = Input.GetAxis("Mouse X");
        inputMouse.y = Input.GetAxis("Mouse Y");

    }

    private void GroundCheck()
    {
        Vector3 start = groundCheck.position + Vector3.up * 0.1f;
        Vector3 end = groundCheck.position + Vector3.down * 0.1f;
        float radius = 0.3f;

        isGrounded = Physics.CheckCapsule(start, end, radius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
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
        float speed = statController.GetStat(PlayerStatController.StatType.Speed);
        Vector3 move = moveDirection * speed;
        velocity.y += gravity * Time.deltaTime;
        Vector3 finalMove = move + velocity;
        velocity = LimitHorizontalVelocity(velocity, statController.GetStat(PlayerStatController.StatType.MaxSpeed));
        controller.Move(finalMove * Time.deltaTime);
        ApplyGroundDrag();
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

    private Vector3 LimitHorizontalVelocity(Vector3 velocity, float maxSpeed)
    {
        Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);

        if (horizontal.magnitude > maxSpeed)
        {
            horizontal = horizontal.normalized * maxSpeed;
        }

        return new Vector3(horizontal.x, velocity.y, horizontal.z);
    }

    private void ApplyGroundDrag()
    {
        if (isGrounded && moveDirection == Vector3.zero)
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, 1 * Time.deltaTime);
        }
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
