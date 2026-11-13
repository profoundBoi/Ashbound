using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [Header("Settings")]
    public int coinValue = 1;
    public AudioClip collectSound;
    public GameObject collectEffect;

    private bool collected;

    private void OnTriggerEnter(Collider other) => TryCollect(other);

    private void OnTriggerStay(Collider other) => TryCollect(other);

    private void TryCollect(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;

        ActionAnimationManager actions = other.GetComponentInParent<ActionAnimationManager>();
        if (actions == null || !actions.IsKicking) return;

        Collect();
    }

    private void Collect()
    {
        collected = true;

        GameManager.Instance.AddCoins(coinValue);

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        if (collectEffect != null)
            Instantiate(collectEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}