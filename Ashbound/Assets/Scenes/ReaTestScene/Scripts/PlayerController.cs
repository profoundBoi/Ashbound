using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Vector3 moveInput;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;

    [Header("Movement")]
    public float speed = 5f;
    public float MaxSpeed;
    public float jumpForce = 5f;
    private float JumpCount;
    [SerializeField]
    private float increaseAmount;
    private int increaseLimit;

    [Header("Tilt (left/right lean)")]
    public float maxTiltAngle = 12f;
    public float tiltSmoothSpeed = 8f;
    private float currentTilt = 0f;
    private float baseYaw;
  

    //PLayer Animations
    [Header("Animations")]
    [SerializeField]
    private Animator playerAnimations;
    private bool isJumping;
    private SpineLean spineScript;
    public int SpeedIndicator;
    public TextMeshProUGUI speedIndicatorText;

    [Header("Trick Settings")]
    public TrickTriggers _trickTriggerScript;
    public float upForceAmount = 10f;
    private UIManager _manager;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        spineScript = GetComponent<SpineLean>();
        SpeedIndicator = 5;
        baseYaw = transform.eulerAngles.y; // remember starting facing direction
        _manager = FindFirstObjectByType<UIManager>();

    }

    // MOVEMENT
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        moveInput = new Vector3(input.x, 0, 0);
    }

    // JUMP
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    public void OnGameSelection(InputAction.CallbackContext context)
    {
        if (context.performed)
            SceneManager.LoadScene("GameSelect");
    }

    void FixedUpdate()
    {
        Vector3 forward = transform.forward * speed;
        Vector3 sideways = transform.right * moveInput.x * speed;

        Vector3 movement = (forward + sideways) * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);

        // bank/tilt left-right based on strafe input, without changing facing direction
        float targetTilt = -moveInput.x * maxTiltAngle;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.fixedDeltaTime * tiltSmoothSpeed);
        rb.MoveRotation(Quaternion.Euler(0f, baseYaw, currentTilt));

        speedIndicatorText.text = SpeedIndicator.ToString();
    }
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void IncreaseSpeedIndicator()
    {
        SpeedIndicator += 5;
    }

    void DecreaseSpeedIndicator()
    {
        SpeedIndicator -= 5;

    }

    public void OnIncreaseSpeed(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (increaseLimit < 5)
            {
                speed += increaseAmount;
                spineScript.OnLean();
                increaseLimit++;
                IncreaseSpeedIndicator();
            }
        }
    }

    public void OnDecreaseSpeed(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (increaseLimit > 0)
            {
                speed -= increaseAmount;
                spineScript.ResetLean();
                increaseLimit--;
                DecreaseSpeedIndicator();
            }
        }
    }

    public void OnButtonNorth(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (_trickTriggerScript != null)
            {
                if (_trickTriggerScript.canRecordButtons)
                {
                    if (_trickTriggerScript.Trickbools[0])
                    {
                        _trickTriggerScript.MoveToNextButton();
                    }
                    else
                    {
                        _manager.FaileTextToShow = _manager._failedText[2];
                        _trickTriggerScript.FailTrick();
                    }
                }
            }
        }
    }

    public void OnButtonSouth(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (_trickTriggerScript != null)
            {
                if (_trickTriggerScript.canRecordButtons)
                {
                    if (_trickTriggerScript.Trickbools[1])
                    {
                        _trickTriggerScript.MoveToNextButton();
                    }
                    else
                    {
                        _manager.FaileTextToShow = _manager._failedText[2];
                        _trickTriggerScript.FailTrick();
                    }
                }
            }
        }
    }

    public void OnButtonEast(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (_trickTriggerScript != null)
            {
                if (_trickTriggerScript.canRecordButtons)
                {
                    if (_trickTriggerScript.Trickbools[2])
                    {
                        _trickTriggerScript.MoveToNextButton();
                    }
                    else
                    {
                        _manager.FaileTextToShow = _manager._failedText[2];
                        _trickTriggerScript.FailTrick();
                    }
                }
            }
        }
    }

    public void OnButtonWest(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (_trickTriggerScript != null)
            {
                if (_trickTriggerScript.canRecordButtons)
                {
                    if (_trickTriggerScript.Trickbools[3])
                    {
                        _trickTriggerScript.MoveToNextButton();
                    }
                    else
                    {
                        _manager.FaileTextToShow = _manager._failedText[2];
                        _trickTriggerScript.FailTrick();
                    }
                }
            }
        }
    }
}