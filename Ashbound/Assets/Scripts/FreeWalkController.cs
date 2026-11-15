using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FreeWalkController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    public float speed = 5f;
    public float runMultiplier = 1.6f;
    public float jumpForce = 5f;

    [Header("Stick Turning")]
    public float stickTurnSpeed = 90f;     // degrees per second at full stick, lower = slower turn
    public bool strafeWithStick = false;   // true = left/right also slides sideways like before

    private Vector3 moveInput;
    private bool isRunning;
    private MissionPoint currentMission;

    [Header("Look (Cinemachine)")]
    public float lookSensitivity = 120f;
    [SerializeField] private Transform cinemachineCameraTarget;
    public float minLookX = -60f;
    public float maxLookX = 60f;

    private Vector2 lookInput;
    private float xRotation;
    private float pendingYaw;

    [Header("Ground Check")]
    public float groundCheckDistance = 1.1f;
    public LayerMask groundMask = ~0;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0f, input.y);

        if (_animator != null)
        {
            bool isMoving = input != Vector2.zero;
            _animator.SetBool("Walk", isMoving);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                jumpForce,
                rb.linearVelocity.z
            );
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRunning = true;
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
    }

    public void OnStartMission(InputAction.CallbackContext context)
    {
        if (context.performed && currentMission != null)
        {
            currentMission.StartMission();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        MissionPoint mission = other.GetComponent<MissionPoint>();

        if (mission != null)
        {
            currentMission = mission;
            mission.Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        MissionPoint mission = other.GetComponent<MissionPoint>();

        if (mission != null)
        {
            if (currentMission == mission)
            {
                currentMission = null;
            }

            mission.Close();
        }
    }

    void Update()
    {
        pendingYaw += lookInput.x * lookSensitivity * Time.deltaTime;
        pendingYaw += moveInput.x * stickTurnSpeed * Time.deltaTime;   // left stick turns the player
        HandleLookPitch();
    }

    void FixedUpdate()
    {
        if (pendingYaw != 0f)
        {
            rb.MoveRotation(
                rb.rotation * Quaternion.Euler(0f, pendingYaw, 0f)
            );

            pendingYaw = 0f;
        }

        float currentSpeed = isRunning ? speed * runMultiplier : speed;

        Vector3 moveVector = new Vector3(strafeWithStick ? moveInput.x : 0f, 0f, moveInput.z);

        Vector3 move = rb.position +
                       transform.TransformDirection(moveVector) *
                       currentSpeed *
                       Time.fixedDeltaTime;

        rb.MovePosition(move);
    }

    private void HandleLookPitch()
    {
        if (cinemachineCameraTarget == null)
            return;

        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookX, maxLookX);

        cinemachineCameraTarget.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundMask
        );
    }
}