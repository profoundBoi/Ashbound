using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinUI : MonoBehaviour
{
    public string prefix = "Caps: ";

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnCoinsChanged += Refresh;
        Refresh(GameManager.Instance.CoinCount);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCoinsChanged -= Refresh;
    }

    private void Refresh(int coins)
    {
        text.text = prefix + coins;
    }
}