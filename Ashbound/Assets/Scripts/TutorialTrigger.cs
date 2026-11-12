using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class TutorialTrigger : MonoBehaviour
{
    [Header("This Lesson's Panels (shown in order)")]
    [Tooltip("Drag in this lesson's panel GameObjects. Each one is its own UI you design.")]
    [SerializeField] private GameObject[] panels;

    [Header("Behaviour")]
    [SerializeField] private string playerTag = "Player";
    [Tooltip("Freeze the game while panels are open.")]
    [SerializeField] private bool pauseWhileOpen = true;
    [Tooltip("Only show this lesson once.")]
    [SerializeField] private bool triggerOnlyOnce = true;
    [Tooltip("Also allow Space on keyboard (handy for testing).")]
    [SerializeField] private bool allowKeyboardFallback = true;

    [Header("Events")]
    [Tooltip("Fires when the first panel opens (e.g. disable player movement).")]
    public UnityEvent onTutorialShown;
    [Tooltip("Fires after the last panel is confirmed (start the lesson objective).")]
    public UnityEvent onLessonStarted;

    private InputAction continueAction;
    private int currentIndex = -1;
    private bool isOpen;
    private bool hasTriggered;
    private float previousTimeScale = 1f;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        HideAllPanels();

        continueAction = new InputAction("TutorialContinue", InputActionType.Button);
        continueAction.AddBinding("<Gamepad>/buttonSouth");
        if (allowKeyboardFallback)
            continueAction.AddBinding("<Keyboard>/space");

        continueAction.performed += OnContinuePressed;
    }

    private void OnDestroy()
    {
        continueAction.performed -= OnContinuePressed;
        continueAction.Dispose();

        if (isOpen && pauseWhileOpen)
            Time.timeScale = previousTimeScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;
        if (triggerOnlyOnce && hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        StartTutorial();
    }

    private void StartTutorial()
    {
        hasTriggered = true;
        isOpen = true;
        currentIndex = -1;

        if (pauseWhileOpen)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        continueAction.Enable();
        onTutorialShown?.Invoke();
        ShowNextPanel();
    }

    private void OnContinuePressed(InputAction.CallbackContext ctx)
    {
        if (!isOpen) return;
        ShowNextPanel();
    }

    private void ShowNextPanel()
    {
        if (currentIndex >= 0 && currentIndex < panels.Length && panels[currentIndex] != null)
            panels[currentIndex].SetActive(false);

        currentIndex++;

        while (currentIndex < panels.Length && panels[currentIndex] == null)
            currentIndex++;

        if (currentIndex >= panels.Length)
        {
            EndTutorial();
            return;
        }

        panels[currentIndex].SetActive(true);
    }

    private void EndTutorial()
    {
        isOpen = false;
        continueAction.Disable();
        HideAllPanels();

        if (pauseWhileOpen)
            Time.timeScale = previousTimeScale;

        onLessonStarted?.Invoke();
    }

    private void HideAllPanels()
    {
        if (panels == null) return;
        foreach (var p in panels)
            if (p != null) p.SetActive(false);
    }
}