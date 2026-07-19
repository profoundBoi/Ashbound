using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [Header("Settings")]
    public int coinValue = 1;
    public AudioClip collectSound;
    public GameObject collectEffect; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddCoins(coinValue);

            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}