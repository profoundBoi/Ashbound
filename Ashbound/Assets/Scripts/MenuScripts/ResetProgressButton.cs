using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetProgressButton : MonoBehaviour
{
    public void ResetProgress()
    {
        ProgressManager.ResetAll();

        // Per your comment in ProgressManager, reset in-memory values too
        if (GameManager.Instance != null)
            GameManager.Instance.ResetTools();

        // Reload the current scene so the UI (level locks, counters) refreshes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}