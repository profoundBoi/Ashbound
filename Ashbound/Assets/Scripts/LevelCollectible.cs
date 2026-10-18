using UnityEngine;

public class LevelCollectible : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Unique ID for this level's item, e.g. Level1. Must match 'Required Collectible Id' on the NEXT level's MissionPoint.")]
    public string levelId = "Level1";
    public AudioClip collectSound;
    public GameObject collectEffect;

    private void Start()
    {
        // Already collected before: never show it again
        if (ProgressManager.HasCollectible(levelId))
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ProgressManager.SetCollectible(levelId);

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        if (collectEffect != null)
            Instantiate(collectEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}