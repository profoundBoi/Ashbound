
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

    [Header("Controller Navigation")]
    [SerializeField] private GameObject firstSelectedButton;

    [Header("Scene Settings")]
    [SerializeField] private string menuSceneName = "Menu";

    [Header("Options")]
    [SerializeField] private bool pauseAudio = true;
    [SerializeField] private bool manageCursor = true;

    public bool IsPaused { get; private set; }

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

#if ENABLE_INPUT_SYSTEM
        pauseAction = new InputAction("Pause", InputActionType.Button);
        pauseAction.AddBinding("<Gamepad>/start");
        pauseAction.AddBinding("<Keyboard>/escape");
        pauseAction.performed += _ => TogglePause();
#endif
    }

    private void Start()
    {
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

#if ENABLE_INPUT_SYSTEM
        pauseAction?.Dispose();
#endif

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

#if !ENABLE_INPUT_SYSTEM
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            TogglePause();
        }
    }
#endif

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

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        IsPaused = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(menuSceneName);
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;

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

        if (paused && firstSelectedButton != null)
        {
            EventSystem.current?.SetSelectedGameObject(null);
            EventSystem.current?.SetSelectedGameObject(firstSelectedButton);
        }
        else if (!paused)
        {
            EventSystem.current?.SetSelectedGameObject(null);
        }
    }
}
