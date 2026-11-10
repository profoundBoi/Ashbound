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
        if (context.canceled)
        {
            StartCoroutine(Kick());
        }
    }

    IEnumerator Kick()
    {
        playerAnimator.SetBool(KickAnimation, true);
        yield return new WaitForSeconds(KickTime);
        playerAnimator.SetBool(KickAnimation, false);
    }
}
