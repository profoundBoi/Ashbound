using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
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
            StartCoroutine(Kick());
        }
        if (context.canceled)
        {
            return;
        }
    }

    IEnumerator Kick()
    {
        playerAnimator.SetBool(KickAnimation, true);
        yield return new WaitForSeconds(KickTime);
        playerAnimator.SetBool(KickAnimation, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kick"))
        {
            print("KIck");
            BurstPipes();
            StartCoroutine(Kick());
        }
    }

    void BurstPipes()
    {
        _pipeAnimator.SetBool("Burst", true);
    }

 
}
