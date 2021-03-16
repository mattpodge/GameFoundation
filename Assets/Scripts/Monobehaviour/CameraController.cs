using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Input Settings")]
    public PlayerInput cameraInput;
    private Vector2 rawInputMovement;
    private Vector2 lookDir;

    public float lookSensitivity = 5f;

    private float yaw;
    private float pitch;

    // Update is called once per frame
    void Update()
    {
        yaw += lookDir.x * lookSensitivity;
        pitch -= lookDir.y * lookSensitivity;

        Vector3 targetRotation = new Vector3(pitch, yaw);
        transform.eulerAngles = targetRotation;
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector2(inputMovement.x, inputMovement.y);
        lookDir = new Vector2(rawInputMovement.x, rawInputMovement.y).normalized;
    }
}
