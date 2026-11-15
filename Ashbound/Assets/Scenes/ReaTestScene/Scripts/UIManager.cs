
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

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
    public GameObject StartPanel;
    public GameObject _startButton;
    public TMP_Text CountdownText;
    public int CountdownSeconds = 3;

    [Header("Countdown Audio")]
    public AudioSource CountdownAudioSource;
    public AudioClip CountdownSound;
    public AudioClip GoSound;

    private void Start()
    {
        Time.timeScale = 0f;

        StartPanel.SetActive(true);

        if (CountdownText != null)
            CountdownText.gameObject.SetActive(false);

        if (_eventSystem != null)
            _eventSystem.SetSelectedGameObject(_startButton);
    }

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

            if (CountdownAudioSource != null && CountdownSound != null)
            {
                CountdownAudioSource.PlayOneShot(CountdownSound);
            }

            yield return new WaitForSecondsRealtime(1f);
        }

        CountdownText.gameObject.SetActive(false);

        if (CountdownAudioSource != null && GoSound != null)
        {
            CountdownAudioSource.PlayOneShot(GoSound);
        }

        Time.timeScale = 1f;
    }

    public void ShowRetryPanel()
    {
        StartCoroutine(DelayRetryPanel());
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        string CurrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(CurrentScene);
    }

    public void LeaveLevel()
    {
        Time.timeScale = 1f;
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

        if (_eventSystem != null)
            _eventSystem.SetSelectedGameObject(_retryButton);
    }
}
