using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputLock : MonoBehaviour
{
    [SerializeField] private bool lockOnStart = true;

    private PlayerInput playerInput;
    public bool IsLocked { get; private set; }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Wait one frame so PlayerController3D.Start() and PlayerInput have finished initialising
    IEnumerator Start()
    {
        yield return null;
        if (lockOnStart) Lock();
    }

    public void Lock()
    {
        playerInput.DeactivateInput();
        IsLocked = true;
    }

    public void Unlock()
    {
        playerInput.ActivateInput();
        IsLocked = false;
    }
}