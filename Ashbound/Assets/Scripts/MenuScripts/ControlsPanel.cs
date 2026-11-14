using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] GameObject panel;    
    [SerializeField] Button closeButton;  

    WorldButton[] menuButtons;
    bool[] previousInteractable;
    GameObject opener;                  

    public bool IsOpen => panel != null && panel.activeSelf;

    void Awake()
    {
        if (panel != null) panel.SetActive(false);
        if (closeButton != null) closeButton.onClick.AddListener(Close);
    }

    void OnDestroy()
    {
        if (closeButton != null) closeButton.onClick.RemoveListener(Close);
    }

    void Update()
    {
        if (!IsOpen) return;

        bool east = Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame;
        bool esc = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
        if (east || esc) Close();
    }

    public void Open()
    {
        if (IsOpen) return;

        opener = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;

        menuButtons = FindObjectsByType<WorldButton>(FindObjectsSortMode.None);
        previousInteractable = new bool[menuButtons.Length];
        for (int i = 0; i < menuButtons.Length; i++)
        {
            previousInteractable[i] = menuButtons[i].interactable;
            menuButtons[i].interactable = false;
        }

        panel.SetActive(true);

        if (closeButton != null && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
    }

    public void Close()
    {
        if (!IsOpen) return;

        panel.SetActive(false);

        for (int i = 0; i < menuButtons.Length; i++)
            if (menuButtons[i] != null)
                menuButtons[i].interactable = previousInteractable[i];

        if (opener != null && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(opener);
    }
}