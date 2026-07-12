using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController3D2 : MonoBehaviour
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
    public float upForceAmount = 10f;

    //PLayer Animations
    [Header("Animations")]
    [SerializeField]
    private Animator playerAnimations;
    private bool isJumping;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
    }

    // MOVEMENT
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = new Vector3(1, 0, 0);
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
        Vector3 move = rb.position + transform.TransformDirection(moveInput) * speed * Time.fixedDeltaTime;
        rb.MovePosition(move);
    }
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UpForce"))
        {
            rb.AddForce(Vector3.up * upForceAmount, ForceMode.Impulse);
        }
    }
}