using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputLock : MonoBehaviour
{
    [SerializeField] private bool lockOnStart = true;

    [Header("UI")]
    [Tooltip("Hidden while input is locked, shown again when unlocked.")]
    [SerializeField] private GameObject[] uiToHideWhileLocked;

    private PlayerInput playerInput;
    public bool IsLocked { get; private set; }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (lockOnStart) SetUIActive(false);
    }

    IEnumerator Start()
    {
        yield return null;
        if (lockOnStart) Lock();
    }

    public void Lock()
    {
        playerInput.DeactivateInput();
        SetUIActive(false);
        IsLocked = true;
    }

    public void Unlock()
    {
        playerInput.ActivateInput();
        SetUIActive(true);
        IsLocked = false;
    }

    private void SetUIActive(bool active)
    {
        foreach (var ui in uiToHideWhileLocked)
        {
            if (ui != null) ui.SetActive(active);
        }
    }
}