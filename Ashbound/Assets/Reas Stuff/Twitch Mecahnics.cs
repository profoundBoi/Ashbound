using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TwitchControls : MonoBehaviour
{
    private bool ButtonSouth, ButtonNorth, ButtonEast, ButtonWest;
    public RawImage ButtonSouthUI, ButtonNorthUI, ButtonEastUI, ButtonWestUI;
    [SerializeField] private AnimationManager animationScript;
    private void Update()
    {
        if (ButtonEast)
        {
            ButtonEastUI.color = Color.green;
        }
        else if (ButtonWest)
        {
            ButtonWestUI.color = Color.green;
        }
        else if (ButtonNorth)
        {
            ButtonNorthUI.color = Color.green;
        }
        else if (ButtonSouth)
        {
            ButtonSouthUI.color = Color.green;
        }
    }

    void GenerateButton()
    {
        int RandomNmber = Random.Range(0, 3);
        if (RandomNmber == 0)
        {
            ButtonEast = true;
        }
        else if (RandomNmber == 1)
        {
            ButtonWest = true;
        }
        else if (RandomNmber == 2)
        {
            ButtonNorth = true;
        }
        else if (RandomNmber == 3)
        {
            ButtonSouth = true;

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Generate"))
        {
            GenerateButton();
        }
    }


    public void OnPerformButtonSouth(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (ButtonSouth)
            {
                ButtonSouthUI.color = Color.white;
                animationScript.canPerformTrick = true;
                ButtonSouth = false;

            }
            else { 
                animationScript.canPerformTrick = false; 
                ButtonSouthUI.color = Color.red;
            }
        }
        
    }

    public void OnPerformButtonNorth(InputAction.CallbackContext context)
    {
       if (context.canceled)
        {
            if (ButtonNorth)
            {
                ButtonNorthUI.color = Color.white;
                animationScript.canPerformTrick = true;
                ButtonNorth = false;

            }
            else { 
                animationScript.canPerformTrick = false; 
                ButtonNorthUI.color = Color.red;
            }
        }
        
    }

    public void OnPerformButtonWest(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (ButtonWest)
            {
                ButtonWestUI.color = Color.white;
                animationScript.canPerformTrick = true;
                ButtonWest = false;

            }
            else { 
                animationScript.canPerformTrick = false; 
                ButtonWestUI.color = Color.red;
            
            }

        }
    }

    public void OnPerformButtonEast(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (ButtonEast)
            {
                ButtonEastUI.color = Color.white;
                animationScript.canPerformTrick = true;
                ButtonEast = false;

            }
            else { 
                animationScript.canPerformTrick = false; 
                ButtonEastUI.color= Color.red;
            }
        }
        
    }
}
