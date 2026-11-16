using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    [Header("Controls Panel")] // NEW
    [SerializeField] private GameObject controlsPanel; // NEW
    [SerializeField] private Button controlsButton; // NEW
    [SerializeField] private Button closeControlsButton; // NEW

    [Header("Controller Navigation")]
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private GameObject controlsFirstSelectedButton; // NEW (usually the close button)

    [Header("Scene Settings")]
    [SerializeField] private string menuSceneName = "Menu";

    [Header("Options")]
    [SerializeField] private bool pauseAudio = true;
    [SerializeField] private bool manageCursor = true;

    public bool IsPaused { get; private set; }
    public bool IsControlsOpen { get; private set; } // NEW

#if ENABLE_INPUT_SYSTEM
    private InputAction pauseAction;
#endif

    private void Awake()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(Pause);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitToMenu);

        if (controlsButton != null) // NEW
            controlsButton.onClick.AddListener(OpenControls);

        if (closeControlsButton != null) // NEW
            closeControlsButton.onClick.AddListener(CloseControls);

#if ENABLE_INPUT_SYSTEM
        pauseAction = new InputAction("Pause", InputActionType.Button);
        pauseAction.AddBinding("<Gamepad>/start");
        pauseAction.AddBinding("<Keyboard>/escape");
        pauseAction.performed += OnPausePerformed;
#endif
    }

    private void Start()
    {
        if (controlsPanel != null) // NEW
            controlsPanel.SetActive(false);

        SetPaused(false);
    }

    private void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        pauseAction?.Enable();
#endif
    }

    private void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        pauseAction?.Disable();
#endif
    }

    private void OnDestroy()
    {
        if (pauseButton != null)
            pauseButton.onClick.RemoveListener(Pause);

        if (resumeButton != null)
            resumeButton.onClick.RemoveListener(Resume);

        if (quitButton != null)
            quitButton.onClick.RemoveListener(QuitToMenu);

        if (controlsButton != null) // NEW
            controlsButton.onClick.RemoveListener(OpenControls);

        if (closeControlsButton != null) // NEW
            closeControlsButton.onClick.RemoveListener(CloseControls);

#if ENABLE_INPUT_SYSTEM
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePerformed;
            pauseAction.Dispose();
        }
#endif

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

#if ENABLE_INPUT_SYSTEM
    private void OnPausePerformed(InputAction.CallbackContext ctx) // NEW (replaces the lambda)
    {
        HandlePauseInput();
    }
#endif

#if !ENABLE_INPUT_SYSTEM
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            HandlePauseInput();
        }
    }
#endif

    // NEW: Escape/Start closes the controls panel first, then toggles pause
    private void HandlePauseInput()
    {
        if (IsControlsOpen)
        {
            CloseControls();
            return;
        }

        TogglePause();
    }

    public void TogglePause()
    {
        SetPaused(!IsPaused);
    }

    public void Pause()
    {
        SetPaused(true);
    }

    public void Resume()
    {
        SetPaused(false);
    }

    // NEW
    public void OpenControls()
    {
        if (!IsPaused || controlsPanel == null)
            return;

        IsControlsOpen = true;
        controlsPanel.SetActive(true);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        SelectButton(controlsFirstSelectedButton != null
            ? controlsFirstSelectedButton
            : closeControlsButton != null ? closeControlsButton.gameObject : null);
    }

    // NEW
    public void CloseControls()
    {
        if (!IsControlsOpen)
            return;

        IsControlsOpen = false;

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        if (pausePanel != null && IsPaused)
            pausePanel.SetActive(true);

        SelectButton(firstSelectedButton);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        IsPaused = false;
        IsControlsOpen = false; // NEW

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(menuSceneName);
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;

        // NEW: make sure controls panel never stays open when unpausing
        if (!paused)
        {
            IsControlsOpen = false;
            if (controlsPanel != null)
                controlsPanel.SetActive(false);
        }

        if (pausePanel != null)
            pausePanel.SetActive(paused);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(!paused);

        Time.timeScale = paused ? 0f : 1f;

        if (pauseAudio)
            AudioListener.pause = paused;

        if (manageCursor)
        {
            Cursor.visible = paused;
            Cursor.lockState = paused
                ? CursorLockMode.None
                : CursorLockMode.Locked;
        }

        if (paused)
            SelectButton(firstSelectedButton);
        else
            EventSystem.current?.SetSelectedGameObject(null);
    }

    // NEW: helper to reduce repeated EventSystem code
    private void SelectButton(GameObject target)
    {
        if (EventSystem.current == null)
            return;

        EventSystem.current.SetSelectedGameObject(null);

        if (target != null)
            EventSystem.current.SetSelectedGameObject(target);
    }
}