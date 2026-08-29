using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public EventSystem _eventSystem;
    public GameObject _retryButton;
    [SerializeField]
    private string MainMenuScene;
    public GameObject RetryPanel;
    public List<GameObject> _failedText;
    public GameObject FaileTextToShow;

    public void ShowRetryPanel()
    {
        StartCoroutine(DelayRetryPanel());
    }

    public void RetryLevel()
    {
        string CurrrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(CurrrentScene);
    }

    public void LeaveLevel()
    {
        SceneManager.LoadScene(MainMenuScene);

    }

    IEnumerator ShowFailingText()
    {
        FaileTextToShow.SetActive(true);
        yield return new WaitForSeconds(2);
        FaileTextToShow.SetActive(false);

    }

    IEnumerator DelayRetryPanel()
    {
        StartCoroutine(ShowFailingText());
        yield return new WaitForSeconds(2);
        RetryPanel.SetActive(true);
        _eventSystem.SetSelectedGameObject(_retryButton);
    }
}
