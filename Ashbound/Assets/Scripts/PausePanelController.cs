using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PausePanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject firstSelectedButton;

    [Header("Scene Names")]
    [SerializeField] private string hubSceneName = "Hub";

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void ShowPanel()
    {
        if (pausePanel == null) return;

        pausePanel.SetActive(true);
        Time.timeScale = 0f; 

        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void HidePanel()
    {
        if (pausePanel == null) return;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
            GameManager.Instance = null;
        }

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name, LoadSceneMode.Single);
    }

    public void ReturnToHub()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(hubSceneName);
    }
}