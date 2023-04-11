using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float smoothTurnTime = 0.1f;

    private CharacterController characterController;
    private Vector3 direction = Vector3.zero;

    private float turnVelcoity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (characterController == null || cameraTransform == null)
            return;

        if (direction.magnitude >= 0.1f) { 
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelcoity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            Vector3 dir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(speed * Time.fixedDeltaTime * dir);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        direction = new Vector3(inputValue.x, 0.0f, inputValue.y).normalized;
    }
}
