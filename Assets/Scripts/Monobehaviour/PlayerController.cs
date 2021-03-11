using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Input Settings")]
    public PlayerInput playerActions;

    private Vector3 rawInputMovement;

    void Update()
    {
        transform.Translate(rawInputMovement * 10 * Time.deltaTime);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 movementInput = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(movementInput.x, 0, 0);

    }

}
