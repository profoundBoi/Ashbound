using System.Collections;
using UnityEngine;

public class ActionAnimationManager : MonoBehaviour
{

    public Animator playerAnimator;

    [Header("Glide Settings")]
    [SerializeField]
    private float GlideTime;
    public string GlideAnimation;


    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        StartCoroutine(GlideToRun());
    }

    IEnumerator GlideToRun()
    {
        yield return new WaitForSeconds(GlideTime);
        playerAnimator.SetBool(GlideAnimation, false);
    }
}
