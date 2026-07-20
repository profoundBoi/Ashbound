using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Trigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject firstSelectedButton; 

    [Header("Scene Names")]
    [SerializeField] private string hubSceneName = "Hub";

    [Header("Trigger Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool onlyTriggerOnce = true;

    private bool hasTriggered = false;

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onlyTriggerOnce && hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;
        ShowPanel();
    }

    public void ShowPanel()
    {
        if (panel == null) return;

        panel.SetActive(true);
        Time.timeScale = 0f;

        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void HidePanel()
    {
        if (panel == null) return;
        panel.SetActive(false);
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