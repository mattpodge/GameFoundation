using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;

    [Header("Input Settings")]
    public PlayerInput playerInput;
    private Vector2 rawInputMovement;
    private Vector3 moveDir;
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


    private void Awake()
    {
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
        float targetSpeed = (isGrounded ? groundSpeed : airSpeed) * moveDir.magnitude;
        playerSpeed = Mathf.SmoothDamp(playerSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // Simply move our player based on input directions
        controller.Move(moveDir * playerSpeed * Time.deltaTime);

        // Make the player face the direction of travel
        if (moveDir != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        }

    }

    void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(playerJumpHeight * -2f * gravity);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector2(inputMovement.x, inputMovement.y);
        moveDir = new Vector3(rawInputMovement.x, 0f, rawInputMovement.y).normalized;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed && isGrounded)
        {
            Jump();
        }
    }

}
