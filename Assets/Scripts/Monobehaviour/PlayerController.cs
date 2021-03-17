using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    private Vector3 rawInputMovement;
    private Vector3 playerVelocity;

    public float playerJumpHeight = 2f;
    private float gravity = Physics.gravity.y;

    public float speedSmoothTime = 0.2f;
    private float speedSmoothVelocity;
    public float groundSpeed = 8f;
    public float airSpeed = 6f;
    private float playerSpeed;

    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;

    [Header("Grounded Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded = true;

    private Transform cameraT;


    private void Awake()
    {
        cameraT = Camera.main.transform;
    }

    void Update()
    {
        // Check our plyer is grounded with a child object
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        // Set downward velocity when the player is on the ground
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Accumulate downward velocity over time whilst in the air and apply it
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Control speed based on if we're on the ground or in the air
        float targetSpeed = (isGrounded ? groundSpeed : airSpeed) * rawInputMovement.magnitude;
        playerSpeed = Mathf.SmoothDamp(playerSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        if (rawInputMovement != Vector3.zero)
        {
            // Rotate player based on where the camera is pointing
            float targetAngle = Mathf.Atan2(rawInputMovement.x, rawInputMovement.z) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Set forward as the angle of the camera
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Move the player
            controller.Move(moveDir * playerSpeed * Time.deltaTime);
        }

    }

    void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(playerJumpHeight * -2f * gravity);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0f, inputMovement.y);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed && isGrounded)
        {
            Jump();
        }
    }

}
