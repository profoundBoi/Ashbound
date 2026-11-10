using UnityEngine;

// Reads PlayerController.SpeedIndicator (5 to 30) and scales particle emission.
// Does not modify PlayerController.
public class SpeedLines : MonoBehaviour
{
    public ParticleSystem lines;

    [Header("Speed range (matches SpeedIndicator)")]
    public float minSpeed = 5f;
    public float maxSpeed = 30f;

    [Header("Lines")]
    public float maxEmissionRate = 100f;
    [Tooltip("0 = lines at every speed, 0.5 = only above half speed")]
    [Range(0f, 1f)] public float startAt = 0.2f;

    private PlayerController player;
    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        emission = lines.emission;
    }

    void Update()
    {
        // Keep looking until the player exists (handles spawned players)
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
            if (player == null) return;
        }

        float t = Mathf.InverseLerp(minSpeed, maxSpeed, player.SpeedIndicator);
        float lineT = Mathf.InverseLerp(startAt, 1f, t);
        emission.rateOverTime = lineT * maxEmissionRate;
    }
}