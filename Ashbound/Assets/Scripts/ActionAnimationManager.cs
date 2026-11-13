using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionAnimationManager : MonoBehaviour
{
    public Animator playerAnimator;

    [Header("Glide Settings")]
    [SerializeField]
    private float GlideTime;
    public string GlideAnimation;

    [Header("Kick Settings")]
    [SerializeField]
    public float KickTime;
    public string KickAnimation;
    [Tooltip("Kick duration never drops below this, even after speed-ups reduce KickTime.")]
    public float MinKickTime = 0.1f;

    public bool IsKicking { get; private set; }
    private Coroutine kickRoutine;

    [Header("Pipe Settings")]
    [SerializeField]
    private Animator _pipeAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        StartCoroutine(GlideToRun());
    }

    IEnumerator GlideToRun()
    {
        yield return new WaitForSeconds(3.5f);
        playerAnimator.SetBool(GlideAnimation, true);
        yield return new WaitForSeconds(GlideTime);
        playerAnimator.SetBool(GlideAnimation, false);
    }

    public void OnKick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartKick();
        }
    }

    void StartKick()
    {
        // Restart if a kick is already running so the flag/animation never get stuck
        if (kickRoutine != null)
            StopCoroutine(kickRoutine);

        kickRoutine = StartCoroutine(Kick());
    }

    IEnumerator Kick()
    {
        IsKicking = true;
        playerAnimator.SetBool(KickAnimation, true);

        yield return new WaitForSeconds(Mathf.Max(KickTime, MinKickTime));

        playerAnimator.SetBool(KickAnimation, false);
        IsKicking = false;
        kickRoutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kick"))
        {
            BurstPipes();
            StartKick();
        }
    }

    void BurstPipes()
    {
        _pipeAnimator.SetBool("Burst", true);
    }
}