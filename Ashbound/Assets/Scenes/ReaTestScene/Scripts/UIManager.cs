using System.Collections;
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

    IEnumerator DelayRetryPanel()
    {
        yield return new WaitForSeconds(1);
        RetryPanel.SetActive(true);
        _eventSystem.SetSelectedGameObject(_retryButton);
    }
}
