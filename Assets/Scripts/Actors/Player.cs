using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Agent agent;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float smoothTurnTime = 0.1f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1;

    private CharacterController controller;
    
    private Vector3 direction = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    private float turnVelcoity = 0f;
    private bool followPlayer = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (controller == null || cameraTransform == null)
            return;

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = 0;

        if (direction.magnitude >= 0.1f) { 
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelcoity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            Vector3 dir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(speed * Time.deltaTime * dir.normalized);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        direction = new Vector3(inputValue.x, 0.0f, inputValue.y).normalized;
    }

    public void OnFollowPlayer(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            followPlayer = !followPlayer;
            agent.FollowPlayer.value = followPlayer;
            agent.StandHere.value = false;
        }
    }

    public void OnStandHere(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            agent.StandHere.value = true;
            followPlayer = false;
            agent.FollowPlayer.value = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.performed && controller.isGrounded) 
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }
}
