using System.ComponentModel;
using Unity.Collections;
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
    [SerializeField] private float damping = 0.5f;
    [SerializeField] private Vector3 velocity = Vector3.zero;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.25f;

    private CharacterController controller;
    //private Rigidbody rb;

    private Vector3 input = Vector3.zero;

    private float turnSpeed = 2f;
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
            velocity.y = 0f;

        if (input.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSpeed, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);

            Vector3 dir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            velocity = speed * dir.normalized;
        }
        else
        {
            velocity.x *= 1f / (1f + (damping * Time.deltaTime));
            velocity.z *= 1f / (1f + (damping * Time.deltaTime));
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        input = new Vector3(inputValue.x, 0.0f, inputValue.y);
    }

    public void OnFollowPlayer(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {

        }
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
            velocity.y += jump;
        }
    }
}
