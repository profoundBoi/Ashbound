using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Pause Panel")]
    [SerializeField]
    private GameObject _pausePanel, _pauseFirstSelect;

    [Header("Controls Panel")]
    [SerializeField]
    private GameObject _controlsPanel, _controlsFirstSelected;

    [Header("Audio")]
    [SerializeField]
    private AudioSource _BGMusic;

    [Header("Event System")]
    [SerializeField]
    private EventSystem _eventSystem;

    [Header("Player input")]
    [SerializeField]
    private PlayerInput _playerInput;

    private void Start()
    {
        if (_playerInput == null)
            _playerInput = FindFirstObjectByType<PlayerInput>();

        if (_eventSystem == null)
            _eventSystem = EventSystem.current;
    }

    private void SelectButton(GameObject button)
    {
        if (_eventSystem == null || button == null) return;

        _eventSystem.SetSelectedGameObject(null);   // clear first so the highlight refreshes
        _eventSystem.SetSelectedGameObject(button);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        if (_playerInput != null)
            _playerInput.actions.FindActionMap("Player")?.Disable();
        _pausePanel.SetActive(true);
        SelectButton(_pauseFirstSelect);
        if (_BGMusic != null) _BGMusic.Pause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (_playerInput != null)
        {
            _playerInput.ActivateInput();
            _playerInput.actions.FindActionMap("Player")?.Enable();
        }
        _pausePanel.SetActive(false);
        if (_BGMusic != null) _BGMusic.UnPause();
    }

    public void OpenControls()
    {
        _pausePanel.SetActive(false);
        _controlsPanel.SetActive(true);
        SelectButton(_controlsFirstSelected);
    }

    public void CloseControls()
    {
        _pausePanel.SetActive(true);
        _controlsPanel.SetActive(false);
        SelectButton(_pauseFirstSelect);
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;   // otherwise the next scene starts frozen
        SceneManager.LoadScene("SafeZone");
    }
}