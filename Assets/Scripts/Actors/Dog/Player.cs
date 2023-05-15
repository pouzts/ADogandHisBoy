using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float smoothTurnTime = 0.1f;
    
    [Header("Physics")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.25f;

    private CharacterController controller;
    //private Rigidbody rb;

    private Vector3 input = Vector3.zero;
    private Vector3 gravityVelocity = Vector3.zero;
    private Vector3 dir = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    private float turnVelcoity = 0f;
    private bool isGrounded = false;

    private float maxSpeed = 0f;
    private float minSpeed = 0f;
    private float speedTime = 0.5f;


    private void Start()
    {
        maxSpeed = speed;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {   
        if (controller == null || cameraTransform == null)
            return;
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded)
            gravityVelocity.y = 0f;

        if (input.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelcoity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            dir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            velocity = Mathf.Lerp(minSpeed, maxSpeed, speedTime) * dir.normalized;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity = Mathf.Lerp(maxSpeed, minSpeed, speedTime) * dir.normalized;
            if (velocity.magnitude <= 0.1f) return;
            controller.Move(velocity * Time.deltaTime);
        }

        gravityVelocity.y += gravity * Time.deltaTime;
        controller.Move(gravityVelocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        if (inputValue.magnitude >= 0.1f)
        {
            input = new Vector3(inputValue.x, 0.0f, inputValue.y).normalized;
        }
    }

    public void OnFollowPlayer(InputAction.CallbackContext context) 
    {
        
    }

    public void OnStandHere(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
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
