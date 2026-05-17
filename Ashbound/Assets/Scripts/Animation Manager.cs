using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private string animationToPlay;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private List<string> AnimationNames;
    public bool canPerformTrick = true;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void PlayNewAnimation()
    {
        playerAnimator.SetBool(animationToPlay, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (canPerformTrick)
            {
                playerAnimator.SetBool(animationToPlay, false);
                if (AnimationNames.Contains(other.name))
                {
                    Debug.Log(other.name);
                    animationToPlay = other.name;
                    PlayNewAnimation();
                }
                else return;
            }
            else { return; }
        }
    }
}
