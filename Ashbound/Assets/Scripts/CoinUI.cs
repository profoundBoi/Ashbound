using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinUI : MonoBehaviour
{
    public string prefix = "Tools: ";

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnToolChanged += Refresh;
        Refresh(GameManager.Instance.ToolCount);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnToolChanged -= Refresh;
    }

    private void Refresh(int tools)
    {
        text.text = prefix + tools;
    }
}