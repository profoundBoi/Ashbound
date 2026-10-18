using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro; // remove this and change TMP_Text to UnityEngine.UI.Text if you use legacy UI

public class UIManager : MonoBehaviour
{
    public EventSystem _eventSystem;
    public GameObject _retryButton;
    [SerializeField]
    private string MainMenuScene;
    public GameObject RetryPanel;
    public List<GameObject> _failedText;
    public GameObject FaileTextToShow;

    [Header("Start / Countdown")]
    public GameObject StartPanel;          // panel containing the "Press to Start" button
    public GameObject _startButton;        // the button itself (for controller/keyboard focus)
    public TMP_Text CountdownText;         // text that shows 3, 2, 1
    public int CountdownSeconds = 3;

    private void Start()
    {
        // Freeze the scene as soon as it loads
        Time.timeScale = 0f;

        StartPanel.SetActive(true);
        if (CountdownText != null) CountdownText.gameObject.SetActive(false);
        _eventSystem.SetSelectedGameObject(_startButton);
    }

    // Hook this to the Start button's OnClick
    public void OnStartPressed()
    {
        StartPanel.SetActive(false);
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        CountdownText.gameObject.SetActive(true);

        for (int i = CountdownSeconds; i > 0; i--)
        {
            CountdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f); // ignores timeScale
        }

        CountdownText.gameObject.SetActive(false);
        Time.timeScale = 1f; // scene plays now
    }

    public void ShowRetryPanel()
    {
        StartCoroutine(DelayRetryPanel());
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f; // safety, Start() will freeze it again on reload
        string CurrrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(CurrrentScene);
    }

    public void LeaveLevel()
    {
        Time.timeScale = 1f; // so the main menu isn't frozen
        SceneManager.LoadScene(MainMenuScene);
    }

    IEnumerator ShowFailingText()
    {
        FaileTextToShow.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        FaileTextToShow.SetActive(false);
    }

    IEnumerator DelayRetryPanel()
    {
        StartCoroutine(ShowFailingText());
        yield return new WaitForSecondsRealtime(2);
        RetryPanel.SetActive(true);
        _eventSystem.SetSelectedGameObject(_retryButton);
    }
}