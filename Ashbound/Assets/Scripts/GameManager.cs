using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string CoinKey = "TotalCoins";

    public int CoinCount { get; private set; }

    public event Action<int> OnCoinsChanged;

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

        CoinCount = PlayerPrefs.GetInt(CoinKey, 0);
    }

    public void AddCoins(int amount)
    {
        CoinCount += amount;
        Save();
        OnCoinsChanged?.Invoke(CoinCount);
    }

    public bool SpendCoins(int amount)
    {
        if (CoinCount < amount) return false;

        CoinCount -= amount;
        Save();
        OnCoinsChanged?.Invoke(CoinCount);
        return true;
    }

    public void ResetCoins()
    {
        CoinCount = 0;
        Save();
        OnCoinsChanged?.Invoke(CoinCount);
    }

    private void Save()
    {
        PlayerPrefs.SetInt(CoinKey, CoinCount);
        PlayerPrefs.Save();
    }
}