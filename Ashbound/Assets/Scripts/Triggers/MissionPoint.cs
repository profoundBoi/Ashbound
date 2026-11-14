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

    [Header("Original Panel Events (your existing setup)")]
    public UnityEvent onOpen;    // opens your original panel
    public UnityEvent onClose;   // closes your original panel
    public UnityEvent onStart;

    private bool originalPanelShown;

    // ---------- State checks ----------

    public bool IsUnlocked =>
        (string.IsNullOrEmpty(requiredCollectibleId) && unlockCost <= 0)
        || ProgressManager.IsLevelUnlocked(levelId);

    private bool HasRequiredCollectible =>
        string.IsNullOrEmpty(requiredCollectibleId)
        || ProgressManager.HasCollectible(requiredCollectibleId);

    private bool HasEnoughCaps =>
        GameManager.Instance.CoinCount >= unlockCost;

    // ---------- Called by your existing player-range trigger ----------

    public void Open()
    {
        Refresh();
    }

    public void Close()
    {
        if (unlockPanel != null) unlockPanel.SetActive(false);

        if (originalPanelShown)
        {
            originalPanelShown = false;
            onClose?.Invoke();
        }
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
            return;
        }

        // Locked: show the unlock panel
        unlockPanel.SetActive(true);
        unlockTitleText.text = missionName;

        // Point the shared Unlock button at THIS node
        unlockButton.onClick.RemoveAllListeners();
        unlockButton.onClick.AddListener(TryUnlock);

        if (!HasRequiredCollectible)
        {
            unlockInfoText.text = "Find the special item in the previous level to unlock.";
            unlockButton.gameObject.SetActive(false);
        }
        else if (!HasEnoughCaps)
        {
            unlockInfoText.text = $"Unlock cost: {unlockCost} caps\nYou have: {GameManager.Instance.CoinCount} caps";
            unlockButton.gameObject.SetActive(false);
        }
        else
        {
            unlockInfoText.text = $"Unlock cost: {unlockCost} caps\nYou have: {GameManager.Instance.CoinCount} caps";
            unlockButton.gameObject.SetActive(true);
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(unlockButton.gameObject);
        }
    }

    // ---------- Hooked to the Unlock button automatically ----------

    public void TryUnlock()
    {
        if (IsUnlocked || !HasRequiredCollectible) return;
        if (!GameManager.Instance.SpendCoins(unlockCost)) return;

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
}