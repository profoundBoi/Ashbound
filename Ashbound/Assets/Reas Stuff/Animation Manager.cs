using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private string animationToPlay;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private List<string> AnimationNames;
    [SerializeField]
    public List<bool> TrickBools;
    public bool canPerformTrick = false;
    [SerializeField] private PlayerController3D2 playerScript;
    private bool PlayerStop;
    [SerializeField] private CapsuleCollider Cc;

    private bool canIncreaseSpeed;

    [Header("Collider Heights")]
    [SerializeField] private float trickColliderHeight = 1f; // was hardcoded to 0, which broke the collider
    [SerializeField] private float defaultColliderHeight = 3.94f;

    [Header("Stop Settings")]
    [SerializeField] private float stopDeceleration = 8f;
    [SerializeField] private float stopSpeedThreshold = 0.05f; // treat "close enough to 0" as stopped

    [Header("Events")]
    public UnityEvent OnPlayerStopped;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerScript = GetComponent<PlayerController3D2>();

        if (Cc == null)
        {
            Cc = GetComponent<CapsuleCollider>();
        }
    }

    private void Update()
    {
        if (PlayerStop && playerScript != null)
        {
            if (playerScript.speed > stopSpeedThreshold)
            {
                playerScript.speed -= Time.deltaTime * stopDeceleration;
            }
            else
            {
                // Player has actually come to a stop - clear the flag so
                // this branch stops fighting future speed changes (e.g. Run).
                playerScript.speed = 0;
                PlayerStop = false;
                OnPlayerStopped?.Invoke();
            }
        }

        canPerformTrick = TrickBools.All(b => b);

        if (canIncreaseSpeed)
        {
            if (playerScript.speed < 15)
            {
                playerScript.speed += Time.deltaTime * 2;
            }
            else { canIncreaseSpeed = false; }
        }
    }

    void PlayNewAnimation()
    {
        playerAnimator.SetBool(animationToPlay, true);

        // Previously hardcoded to 0, which made the CapsuleCollider degenerate
        // (zero-height) and could break grounding/collision entirely.
        if (Cc != null)
        {
            Cc.height = trickColliderHeight;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (canPerformTrick)
        {
            playerAnimator.SetBool(animationToPlay, false);

            if (!AnimationNames.Contains(other.name))
            {
                return;
            }

            Debug.Log(other.name);
            animationToPlay = other.name;

            // Starting a new trick animation means the player is active again,
            // so any pending stop-decel should not continue fighting it.
            PlayerStop = false;

            PlayNewAnimation();

            if (other.name == "Run")
            {
                canPerformTrick = false;
                for (int i = 0; i < 3 && i < TrickBools.Count; i++)
                {
                    TrickBools[i] = false;
                }

                if (Cc != null)
                {
                    Cc.height = defaultColliderHeight;
                }

                if (playerScript != null)
                {
                    canIncreaseSpeed = true;
                }
            }
        }
        else
        {
            if (AnimationNames.Contains(other.name))
            {
                PlayerStop = true;
                animationToPlay = "Stop";
                playerAnimator.SetBool(animationToPlay, true);
                StartCoroutine(StopPlayer());
            }
        }
    }

    IEnumerator StopPlayer()
    {
        yield return new WaitForSeconds(1);
        OnPlayerStopped?.Invoke();

    }
}