using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float smoothTurnTime = 0.1f;
    [SerializeField] private float gravity = -9.8f;

    public bool FollowPlayer { get; set; } = false;
    public bool StandHere { get; set; } = false;

    private Rigidbody rb;
    private Vector3 direction = Vector3.zero;

    private float turnVelcoity = 0f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rb == null || cameraTransform == null)
            return;

        if (direction.magnitude >= 0.1f) { 
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelcoity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            Vector3 dir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * dir.normalized);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();
        direction = new Vector3(inputValue.x, 0.0f, inputValue.y).normalized;
    }

    public void OnFollowPlayer(InputAction.CallbackContext context) 
    {
        if (context.performed)
            FollowPlayer = !FollowPlayer;
    }
}
