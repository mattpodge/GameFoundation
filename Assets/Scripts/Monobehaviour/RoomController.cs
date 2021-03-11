using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomController : MonoBehaviour
{
    public float rotationSpeed;
    public bool canRotate = true;

    [Header("Input Settings")]
    public PlayerInput roomActions;

    public void OnRotateClockwise(InputAction.CallbackContext value)
    {
        if (value.performed && canRotate)
        {
            StartCoroutine(SmoothRotation(Vector3.up, 90.0f, 0.5f));
        }
    }

    public void OnRotateCounterClockwise(InputAction.CallbackContext value)
    {
        if (value.performed && canRotate)
        {
            StartCoroutine(SmoothRotation(Vector3.up, -90.0f, 0.5f));
        }
    }

    IEnumerator SmoothRotation(Vector3 axis, float byAngle, float duration = 1f)
    {
        Quaternion fromAngle = transform.rotation;
        Quaternion toAngle = transform.rotation * Quaternion.Euler(Vector3.up * byAngle);
        float elapsedTime = 0f;
        canRotate = false;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = toAngle;
        canRotate = true;
    }
}
