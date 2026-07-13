using UnityEngine;
using UnityEngine.Events;

public class MissionPoint : MonoBehaviour
{
    [Header("Mission Info")]
    public string missionName = "New Mission";
    [TextArea] public string missionDescription;
    public Sprite missionThumbnail;

    [Header("Scene / Load Target")]
    [Tooltip("Scene name to load when the player presses Start. Leave blank if you're handling loading yourself via onStart.")]
    public string missionSceneName;

    [Header("Events")]
    [Tooltip("Invoked when the player walks into range — wire this to your panel's 'open and populate' method.")]
    public UnityEvent onOpen;
    [Tooltip("Invoked when the player walks away — wire this to your panel's 'close' method.")]
    public UnityEvent onClose;
    [Tooltip("Invoked when the player presses the Start button on the panel.")]
    public UnityEvent onStart;

    public void Open()
    {
        onOpen?.Invoke();
    }

    public void Close()
    {
        onClose?.Invoke();
    }

    public void StartMission()
    {
        onStart?.Invoke();

        if (!string.IsNullOrEmpty(missionSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(missionSceneName);
        }
    }
}