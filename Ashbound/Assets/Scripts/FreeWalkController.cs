using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A stripped-down "free walk" controller for a hub menu: movement, look,
/// jump, run, and basic locomotion animation, plus walking up to a
/// MissionPoint and pressing a button to view or start it.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FreeWalkController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Mission Interaction")]
    [Tooltip("UI shown/hidden automatically when a mission is in range (e.g. a 'Press E' prompt with the mission name).")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private UnityEngine.UI.Text interactPromptText; // optional, leave empty if not using

    private MissionPoint currentMission;

    [Header("Movement")]
    public float speed = 5f;
    public float runMultiplier = 1.6f;
    public float jumpForce = 5f;

    private Vector3 moveInput;
    private bool isRunning;

    [Header("Look")]
    public float lookSensitivity = 120f;
    [SerializeField] private Transform cameraHolder;
    public float minLookX = -60f;
    public float maxLookX = 60f;

    private Vector2 lookInput;
    private float xRotation;

    [Header("Ground Check")]
    public float groundCheckDistance = 1.1f;
    public LayerMask groundMask = ~0; // defaults to "everything"; set explicitly in Inspector if needed

    [Header("Animation")]
    [SerializeField] private Animator playerAnimations;
    [SerializeField] private List<string> animationBools; // 0=Walk, 1=Run, 2=Jump

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // ---- Input callbacks (wired up via PlayerInput's Unity Events or SendMessages) ----

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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && currentMission != null)
        {
            currentMission.Interact();
        }
    }

    // ---- Mission trigger detection ----
    // Requires a trigger Collider on this GameObject (or a child) and each
    // MissionPoint to have its own trigger Collider tagged/layered so they
    // can be found here. Simplest setup: put a MissionPoint on the mission
    // object with a large-ish trigger Collider (e.g. a sphere the player
    // walks into).

    void OnTriggerEnter(Collider other)
    {
        MissionPoint mission = other.GetComponent<MissionPoint>();
        if (mission == null) return;

        currentMission = mission;
        ShowPrompt(mission);
    }

    void OnTriggerExit(Collider other)
    {
        MissionPoint mission = other.GetComponent<MissionPoint>();
        if (mission == null || mission != currentMission) return;

        currentMission = null;
        HidePrompt();
    }

    private void ShowPrompt(MissionPoint mission)
    {
        if (interactPrompt != null) interactPrompt.SetActive(true);
        if (interactPromptText != null) interactPromptText.text = $"Press E — View \"{mission.missionName}\"";
    }

    private void HidePrompt()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    // ---- Physics / movement ----

    void FixedUpdate()
    {
        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        Vector3 move = rb.position + transform.TransformDirection(moveInput) * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(move);
    }

    void LateUpdate()
    {
        HandleLook();
        HandleAnimation();
    }

    private void HandleLook()
    {
        if (cameraHolder == null) return;

        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookX, maxLookX);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleAnimation()
    {
        if (playerAnimations == null || animationBools == null || animationBools.Count < 3) return;

        bool isMoving = moveInput.x != 0f || moveInput.z != 0f;
        bool grounded = IsGrounded();

        ResetAnimBools();

        if (!grounded)
        {
            playerAnimations.SetBool(animationBools[2], true); // Jump
        }
        else if (isMoving)
        {
            playerAnimations.SetBool(isRunning ? animationBools[1] : animationBools[0], true); // Run / Walk
        }
        // else: idle, all bools already false
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