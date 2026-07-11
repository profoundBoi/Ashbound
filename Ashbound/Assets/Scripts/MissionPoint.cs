using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Put this on each mission "node" in the hub, along with a trigger
/// Collider (e.g. a SphereCollider with "Is Trigger" checked) sized to
/// however close the player needs to walk up to interact.
///
/// When the player is in range and presses Interact, this opens a mission
/// info panel showing name/description with View and Start options.
/// Wire up onView / onStart in the Inspector, or leave them and just use
/// OpenInfoPanel()/StartMission() directly if you'd rather script it.
/// </summary>
public class MissionPoint : MonoBehaviour
{
    [Header("Mission Info")]
    public string missionName = "New Mission";
    [TextArea] public string missionDescription;
    public Sprite missionThumbnail;

    [Header("Scene / Load Target")]
    [Tooltip("Scene name to load when the player confirms Start. Leave blank if you're handling loading yourself via onStart.")]
    public string missionSceneName;

    [Header("Events")]
    [Tooltip("Invoked when the player presses Interact near this mission — typically opens your mission info UI panel.")]
    public UnityEvent<MissionPoint> onView;
    [Tooltip("Invoked when the player confirms Start on this mission (e.g. a button inside the info panel calls StartMission()).")]
    public UnityEvent onStart;

    /// <summary>
    /// Called by FreeWalkController when the player presses Interact while
    /// standing in this mission's trigger zone.
    /// </summary>
    public void Interact()
    {
        onView?.Invoke(this);
    }

    /// <summary>
    /// Hook this up to your info panel's "Start Mission" button.
    /// </summary>
    public void StartMission()
    {
        onStart?.Invoke();

        if (!string.IsNullOrEmpty(missionSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(missionSceneName);
        }
    }
}