using System.ComponentModel;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    //[SerializeField] private float smoothTurnTime = 0.1f;

    [Header("Character Attributes")]
    [SerializeField] private float speed = 10f;

    [Header("Physics")]
    //[SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float damping = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.25f;

    //private CharacterController controller;
    private Rigidbody rb;

    private Vector3 velocity = Vector3.zero;
    private Vector3 input = Vector3.zero;
    
    private bool isGrounded = false;

    private float turnSpeed = 0f;
    private float smoothTurnTime = 0.05f;

    private void Start()
    {
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        //isGrounded = controller.isGrounded;

        //if (controller == null || cameraTransform == null)
        //return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        if (rb == null || cameraTransform == null)
            return;

        if (input.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSpeed, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            Vector3 dir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            velocity = speed * dir.normalized;
        }
        else
        {
            velocity.x *= 1.0f / (1.0f + (damping * Time.fixedDeltaTime));
            velocity.z *= 1.0f / (1.0f + (damping * Time.fixedDeltaTime));
        }

        //if (!isGrounded)
        //    velocity.y += gravity * Time.fixedDeltaTime;
        
        if (isGrounded)
            rb.useGravity = false;
        else
            rb.useGravity = true;

        //controller.Move(velocity * Time.deltaTime);
        rb.position += velocity * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawSphere(groundCheck.position, groundRadius);
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
        if (context.started && isGrounded)
        { 
            float jump = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y * rb.mass);
            rb.AddForce(jump * Time.fixedDeltaTime * Vector3.up, ForceMode.Impulse);
        }
    }
}
