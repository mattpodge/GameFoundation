using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;

    [Header("Input Settings")]
    public PlayerInput playerActions;
    public float playerSpeed = 10f;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    private void Awake()
    {
    }

    void FixedUpdate()
    {
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 movementInput = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(movementInput.x, 0f, movementInput.y);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
    }

}
