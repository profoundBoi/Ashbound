using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string ToolKey = "TotalTools";

    private int savedTools;  
    private int runTools;     

    public int ToolCount => savedTools + runTools;

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

        savedTools = PlayerPrefs.GetInt(ToolKey, 0);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (runTools == 0) return;
        runTools = 0;
        OnToolChanged?.Invoke(ToolCount);
    }

    public void AddTools(int amount)
    {
        runTools += amount;                 
        OnToolChanged?.Invoke(ToolCount);
    }

    public void CommitTools()
    {
        savedTools += runTools;
        runTools = 0;
        Save();
        OnToolChanged?.Invoke(ToolCount);
    }

    public bool SpendTool(int amount)
    {
        if (savedTools < amount) return false;   

        savedTools -= amount;
        Save();
        OnToolChanged?.Invoke(ToolCount);
        return true;
    }

    public void ResetTools()
    {
        savedTools = 0;
        runTools = 0;
        Save();
        OnToolChanged?.Invoke(ToolCount);
    }

    private void Save()
    {
        PlayerPrefs.SetInt(ToolKey, savedTools);
        PlayerPrefs.Save();
    }
}