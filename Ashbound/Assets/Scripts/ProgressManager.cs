using UnityEngine;

public static class ProgressManager
{
    private const string CollectiblePrefix = "Collectible_";
    private const string UnlockPrefix = "LevelUnlocked_";

    public static bool HasCollectible(string levelId)
    {
        return !string.IsNullOrEmpty(levelId) &&
               PlayerPrefs.GetInt(CollectiblePrefix + levelId, 0) == 1;
    }

    public static void SetCollectible(string levelId)
    {
        PlayerPrefs.SetInt(CollectiblePrefix + levelId, 1);
        PlayerPrefs.Save();
    }

    public static bool IsLevelUnlocked(string levelId)
    {
        return PlayerPrefs.GetInt(UnlockPrefix + levelId, 0) == 1;
    }

    public static void UnlockLevel(string levelId)
    {
        PlayerPrefs.SetInt(UnlockPrefix + levelId, 1);
        PlayerPrefs.Save();
    }

    // For testing: wipes everything. Call GameManager.Instance.ResetCoins() after.
    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}