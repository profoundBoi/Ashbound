using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    public List<bool> TrickBools;
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

        canPerformTrick = TrickBools.All(b => b);
        

    }
    void PlayNewAnimation()
    {
        playerAnimator.SetBool(animationToPlay, true);
        CapsuleCollider Cc = GetComponent<CapsuleCollider>();
        Cc.height = 0;
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
                        for(int i = 0; i<3; i++)
                        {
                            TrickBools[i] = false;
                            CapsuleCollider Cc = GetComponent<CapsuleCollider>();
                            Cc.height = 3.94f;
                        }
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
