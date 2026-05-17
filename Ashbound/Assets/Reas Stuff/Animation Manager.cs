using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.LowLevel;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private string animationToPlay;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private List<string> AnimationNames;
    public bool canPerformTrick = false;
    [SerializeField] private PlayerController3D2 playerScript;
    private bool PlayerStop;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerScript = GetComponent<PlayerController3D2>();
    }


    private void Update()
    {
        if (PlayerStop && playerScript != null)
        {
            if(playerScript.speed > 0)
            {
                playerScript.speed -= Time.deltaTime * 8;
            }
        }
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

                    if (other.name == "Run")
                    {
                        canPerformTrick = false;
                    }
                }
                else return;
            }
            else if (!canPerformTrick){

                if (AnimationNames.Contains(other.name))
                {
                    PlayerStop = true;
                    animationToPlay = "Stop";
                    playerAnimator.SetBool(animationToPlay, true);
                }

                
            }
        }
    }
}
