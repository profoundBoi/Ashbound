using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionPoint : MonoBehaviour
{
    [Header("Mission Info")]
    public string missionName = "New Mission";
    [TextArea] public string missionDescription;
    public Sprite missionThumbnail;

    [Header("Scene / Load Target")]
    public string missionSceneName;

    [Header("Unlock Requirements")]
    [Tooltip("Unique ID of THIS level, e.g. Level2.")]
    public string levelId = "Level2";
    [Tooltip("Collectible ID from the PREVIOUS level. Blank for the first level.")]
    public string requiredCollectibleId = "Level1";
    [Tooltip("Caps it costs to unlock. 0 = free.")]
    public int unlockCost = 50;

    [Header("Unlock Panel (shown when level is locked)")]
    public GameObject unlockPanel;
    public TMP_Text unlockTitleText;   // mission name
    public TMP_Text unlockInfoText;    // cost / status messages
    public Button unlockButton;

    [Header("Controller Navigation")]
    [Tooltip("Button selected when the original (unlocked) panel opens, e.g. the Start button.")]
    public GameObject unlockedFirstSelected;
    [Tooltip("Button selected when locked and the player can't unlock yet, e.g. a Close button. Optional.")]
    public GameObject lockedFirstSelected;

    [Header("Pause Button")]
    [Tooltip("The on-screen pause button (PauseBT). Hidden while the player is at this mission point.")]
    public GameObject pauseButtonObject;

    [Header("Original Panel Events (your existing setup)")]
    public UnityEvent onOpen;    // opens your original panel
    public UnityEvent onClose;   // closes your original panel
    public UnityEvent onStart;

    private bool originalPanelShown;
    private bool pauseButtonHidden;

    // ---------- State checks ----------

    public bool IsUnlocked =>
        (string.IsNullOrEmpty(requiredCollectibleId) && unlockCost <= 0)
        || ProgressManager.IsLevelUnlocked(levelId);

    private bool HasRequiredCollectible =>
        string.IsNullOrEmpty(requiredCollectibleId)
        || ProgressManager.HasCollectible(requiredCollectibleId);

    private bool HasEnoughCaps =>
        GameManager.Instance.ToolCount >= unlockCost;

    // ---------- Called by your existing player-range trigger ----------

    public void Open()
    {
        HidePauseButton();
        Refresh();
    }

    public void Close()
    {
        StopAllCoroutines();

        if (unlockPanel != null) unlockPanel.SetActive(false);

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        if (originalPanelShown)
        {
            originalPanelShown = false;
            onClose?.Invoke();
        }

        ShowPauseButton();
    }

    // ---------- Decides which panel to show ----------

    private void Refresh()
    {
        if (IsUnlocked)
        {
            // Unlocked: hide unlock panel, show your original panel
            if (unlockPanel != null) unlockPanel.SetActive(false);

            if (!originalPanelShown)
            {
                originalPanelShown = true;
                onOpen?.Invoke();
            }

            SelectNextFrame(unlockedFirstSelected);
            return;
        }

        // Locked: show the unlock panel
        unlockPanel.SetActive(true);
        unlockTitleText.text = missionName;

        // Point the shared Unlock button at THIS node
        unlockButton.onClick.RemoveAllListeners();
        unlockButton.onClick.AddListener(TryUnlock);

        unlockInfoText.text = $"Unlock cost: {unlockCost} tools\nYou have: {GameManager.Instance.ToolCount} tools";

        if (!HasRequiredCollectible)
        {
            unlockInfoText.text = "Find the special item in the previous level to unlock.";
            unlockButton.gameObject.SetActive(false);
            SelectNextFrame(lockedFirstSelected);
        }
        else if (!HasEnoughCaps)
        {
            unlockButton.gameObject.SetActive(false);
            SelectNextFrame(lockedFirstSelected);
        }
        else
        {
            unlockButton.gameObject.SetActive(true);
            SelectNextFrame(unlockButton.gameObject);
        }
    }

    // ---------- Hooked to the Unlock button automatically ----------

    public void TryUnlock()
    {
        if (IsUnlocked || !HasRequiredCollectible) return;
        if (!GameManager.Instance.SpendTool(unlockCost)) return;

        ProgressManager.UnlockLevel(levelId);
        Refresh(); // switches to your original panel
    }

    // ---------- Hook to your ORIGINAL Start button, same as before ----------

    public void StartMission()
    {
        if (!IsUnlocked) return;

        onStart?.Invoke();

        if (!string.IsNullOrEmpty(missionSceneName))
            SceneManager.LoadScene(missionSceneName);
    }

    // ---------- Pause button helpers ----------

    private void HidePauseButton()
    {
        if (pauseButtonObject == null) return;

        pauseButtonObject.SetActive(false);
        pauseButtonHidden = true;
    }

    private void ShowPauseButton()
    {
        if (pauseButtonObject == null || !pauseButtonHidden) return;

        pauseButtonObject.SetActive(true);
        pauseButtonHidden = false;
    }

    private void OnDisable()
    {
        // Don't leave the pause button hidden if this node gets disabled
        ShowPauseButton();
    }

    // ---------- Selection helpers ----------

    // Waits one frame so the same south press that opened the node
    // isn't also read as "Submit" on the newly selected button.
    private void SelectNextFrame(GameObject target)
    {
        StopAllCoroutines();
        StartCoroutine(SelectRoutine(target));
    }

    private IEnumerator SelectRoutine(GameObject target)
    {
        yield return null;

        if (EventSystem.current == null) yield break;

        EventSystem.current.SetSelectedGameObject(null);

        if (target != null && target.activeInHierarchy)
            EventSystem.current.SetSelectedGameObject(target);
    }
}