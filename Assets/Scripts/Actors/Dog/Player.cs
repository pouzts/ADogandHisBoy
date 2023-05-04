using JetBrains.Rider.Unity.Editor;
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
    
    [Header("Physics")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private Vector3 drag = Vector3.zero;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.25f;

    private CharacterController controller;
    //private Rigidbody rb;

    private Vector3 input = Vector3.zero;
    private Vector3 gravityVelocity = Vector3.zero;

    private float turnVelcoity = 0f;
    private bool followPlayer = false;

    private bool isGrounded = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {   
        if (controller == null || cameraTransform == null)
            return;
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded)
            gravityVelocity.y = 0f;

        if (input.magnitude >= 0.1f) { 
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelcoity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            Vector3 dir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 velocity = speed * dir.normalized;
            velocity.x /= 1f + drag.x * Time.deltaTime;
            velocity.z /= 1f + drag.z * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        gravityVelocity.y += gravity * Time.deltaTime;
        controller.Move(gravityVelocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        input = new Vector3(inputValue.x, 0.0f, inputValue.y).normalized;
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
        if (context.performed && isGrounded) 
        {
            float jump = Mathf.Sqrt(jumpHeight * -2 * gravity);
            gravityVelocity.y += jump;
        }
    }
}
