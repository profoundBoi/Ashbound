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

    //PLayer Animations
    [Header("Animations")]
    [SerializeField]
    private Animator playerAnimations;
    private bool isJumping;
    private SpineLean spineScript;
    [SerializeField]
    private int SpeedIndicator;
    public TextMeshProUGUI speedIndicatorText;

    [Header("Trick Settings")]
    public TrickTriggers _trickTriggerScript;

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
                        _trickTriggerScript.FailTrick();
                    }
                }
            }
        }
    }

}