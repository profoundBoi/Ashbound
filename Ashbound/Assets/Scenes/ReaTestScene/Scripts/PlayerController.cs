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
    [SerializeField]
    private List<string> SlideAnimations;
    private bool isJumping;
    private SpineLean spineScript;
    public int SpeedIndicator;
    public TextMeshProUGUI speedIndicatorText;

    [Header("Trick Settings")]
    public TrickTriggers _trickTriggerScript;
    public float upForceAmount = 10f;
    private UIManager _manager;

    [Header("Lane Switching")]
    public float laneDistance = 2f;
    public float laneSwitchSpeed = 25f;
    private int currentLane = 0;
    private float currentLaneOffset = 0f;
    private bool laneInputReady = true;

    [Header("Shift")]
    public float shiftAmount = 1f;
    public float shiftSpeed = 10f;
    private float extraOffset = 0f;
    private float currentShiftOffset = 0f;

    [Header("Audio")]
    public AudioSource SideSlide;

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
        baseYaw = transform.eulerAngles.y;
        _manager = FindFirstObjectByType<UIManager>();
    }

    // MOVEMENT
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        moveInput = new Vector3(input.x, 0, 0);

        if (Mathf.Abs(input.x) < 0.5f)
        {
            laneInputReady = true;
        }
        else if (laneInputReady)
        {
            int newLane = Mathf.Clamp(currentLane + (input.x > 0 ? 1 : -1), -1, 1);

            if (newLane != currentLane)
            {
                if (newLane < currentLane)
                    StartCoroutine(PlaySlideLeft());
                else
                    StartCoroutine(PlaySlideRight());

                currentLane = newLane;
            }

            laneInputReady = false;
        }
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

    // SHIFT (called by trigger zones)
    public void ShiftLeft()
    {
        extraOffset -= shiftAmount;
    }

    public void ShiftRight()
    {
        extraOffset += shiftAmount;
    }

    void FixedUpdate()
    {
        Vector3 forward = transform.forward * speed;

        float targetLaneOffset = currentLane * laneDistance;
        float targetShiftOffset = extraOffset;

        float newLaneOffset = Mathf.MoveTowards(
            currentLaneOffset,
            targetLaneOffset,
            laneSwitchSpeed * Time.fixedDeltaTime
        );

        float newShiftOffset = Mathf.MoveTowards(
            currentShiftOffset,
            targetShiftOffset,
            shiftSpeed * Time.fixedDeltaTime
        );

        Vector3 sideways = transform.right * (
            (newLaneOffset - currentLaneOffset) +
            (newShiftOffset - currentShiftOffset)
        );

        currentLaneOffset = newLaneOffset;
        currentShiftOffset = newShiftOffset;

        Vector3 movement = forward * Time.fixedDeltaTime + sideways;

        rb.MovePosition(rb.position + movement);

        // bank/tilt left-right based on strafe input, without changing facing direction
        float targetTilt = -moveInput.x * maxTiltAngle;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.fixedDeltaTime * tiltSmoothSpeed);
        rb.MoveRotation(Quaternion.Euler(0f, baseYaw, currentTilt));

        speedIndicatorText.text = SpeedIndicator.ToString();
    }

    IEnumerator PlaySlideLeft()
    {
        SideSlide.Play();
        playerAnimations.SetBool(SlideAnimations[0], true);
        yield return new WaitForSeconds(0.1f);
        playerAnimations.SetBool(SlideAnimations[0], false);
    }

    IEnumerator PlaySlideRight()
    {
        SideSlide.Play();
        playerAnimations.SetBool(SlideAnimations[1], true);
        yield return new WaitForSeconds(0.1f);
        playerAnimations.SetBool(SlideAnimations[1], false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shift"))
        {
           ShiftLeft();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shift"))
        {
            ShiftRight();
        }
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