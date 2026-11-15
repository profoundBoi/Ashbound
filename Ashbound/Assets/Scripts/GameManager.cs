using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string ToolKey = "TotalTools";

    public int ToolCount { get; private set; }

    public event Action<int> OnToolChanged;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance == null)
        {
            var go = new GameObject("GameManager");
            go.AddComponent<GameManager>();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ToolCount = PlayerPrefs.GetInt(ToolKey, 0);
    }

    public void AddTools(int amount)
    {
        ToolCount += amount;
        Save();
        OnToolChanged?.Invoke(ToolCount);
    }

    public bool SpendTool(int amount)
    {
        if (ToolCount < amount) return false;

        ToolCount -= amount;
        Save();
        OnToolChanged?.Invoke(ToolCount);
        return true;
    }

    public void ResetTools()
    {
        ToolCount = 0;
        Save();
        OnToolChanged?.Invoke(ToolCount);
    }

    private void Save()
    {
        PlayerPrefs.SetInt(ToolKey, ToolCount);
        PlayerPrefs.Save();
    }
}