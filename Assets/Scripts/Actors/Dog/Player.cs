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
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float drag = 2f;

    /*[Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.25f;
*/
    private CharacterController controller;
    //private Rigidbody rb;

    private Vector3 input = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    private float turnVelcoity = 0f;
    private bool followPlayer = false;

    private bool isGrounded = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        //if (rb == null || cameraTransform == null)
        //    return;
        
        if (controller == null || cameraTransform == null)
            return;
        
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        isGrounded = controller.isGrounded;

        //if (isGrounded && velocity.y < 0)
        //    velocity.y = 0;

        /*if (isGrounded)
        {
            rb.useGravity = false;
            rb.drag = hDrag;
        }
        else
        {
            rb.useGravity = true;
            rb.drag = vDrag;
        }*/

        if (input.magnitude >= 0.1f) { 
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelcoity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            Vector3 dir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //rb.AddForce(speed * Time.fixedDeltaTime * dir, ForceMode.VelocityChange);
            controller.Move(speed * Time.fixedDeltaTime * dir.normalized);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
            float jump = Mathf.Sqrt(jumpForce * -2 * gravity);
            velocity.y += jump * Time.fixedDeltaTime;
            //rb.AddForce(jump * Vector3.up, ForceMode.Impulse);
        }
    }
}
