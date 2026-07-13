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

    private Vector3 moveInput;
    private bool isRunning;
    private MissionPoint currentMission;

    [Header("Look (Cinemachine)")]
    public float lookSensitivity = 120f;
    [Tooltip("Assign this as the 'Tracking Target' on your CinemachineCamera (the Follow/LookAt target). " +
             "Cinemachine reads this transform's rotation each frame to orbit the camera — this script only " +
             "ever rotates this transform, it never touches the camera itself.")]
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
    [SerializeField] private Animator playerAnimations;
    [SerializeField] private List<string> animationBools; 

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
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
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

        HandleLookPitch();
    }

    void FixedUpdate()
    {
        if (pendingYaw != 0f)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, pendingYaw, 0f));
            pendingYaw = 0f;
        }

        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        Vector3 move = rb.position + transform.TransformDirection(moveInput) * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(move);
    }

    void LateUpdate()
    {
        HandleAnimation();
    }

    private void HandleLookPitch()
    {
        if (cinemachineCameraTarget == null) return;

        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookX, maxLookX);

        cinemachineCameraTarget.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleAnimation()
    {
        if (playerAnimations == null || animationBools == null || animationBools.Count < 3) return;

        bool isMoving = moveInput.x != 0f || moveInput.z != 0f;
        bool grounded = IsGrounded();

        ResetAnimBools();

        if (!grounded)
        {
            playerAnimations.SetBool(animationBools[2], true); 
        }
        else if (isMoving)
        {
            playerAnimations.SetBool(isRunning ? animationBools[1] : animationBools[0], true);
        }

    }

    private void ResetAnimBools()
    {
        for (int i = 0; i < animationBools.Count; i++)
        {
            playerAnimations.SetBool(animationBools[i], false);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }
}