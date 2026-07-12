using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TwitchControls : MonoBehaviour
{
    [SerializeField] private AnimationManager animationScript;

    [SerializeField]
    private List<bool> buttonBools;
    [SerializeField]
    private List<string> buttonNames;
    [SerializeField]
    private List<string> ChosenButtons;
    [SerializeField]
    private List<RawImage> ButtonUi;
    [SerializeField]
    private int CombinationSize;
    public List<GameObject> Buttons;
    public Transform buttonsParent;
    public List<GameObject> ChosenButtonObjects;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (ButtonUi.Count > 0 && ButtonUi[0] != null)
        {
            ButtonUi[0].color = Color.green;
        }
    }

    void GenerateButton()
    {
        ChosenButtons.Clear();
        for (int i = 0; i < CombinationSize; i++)
        {
            ChosenButtons.Add(buttonNames[Random.Range(0, buttonNames.Count)]);
        }
        CheckButtons();
    }

    void CheckButtons()
    {
        foreach (var chosen in ChosenButtons)
        {
            for (int i = 0; i < buttonNames.Count; i++)
            {
                if (chosen == buttonNames[i])
                {
                    GameObject buttonObj = Instantiate(Buttons[i]);
                    buttonObj.transform.SetParent(buttonsParent, false);
                    ChosenButtonObjects.Add(buttonObj);

                    RawImage buttonImage = buttonObj.GetComponent<RawImage>();
                    ButtonUi.Add(buttonImage);
                    break;
                }
            }
        }

        ActivateBools();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Generate"))
        {
            GenerateButton();
        }
    }

    void ActivateBools()
    {
        for (int b = 0; b < buttonBools.Count; b++)
        {
            buttonBools[b] = false;
        }

        if (ChosenButtons.Count == 0) return;

        int idx = buttonNames.IndexOf(ChosenButtons[0]);
        if (idx >= 0)
        {
            buttonBools[idx] = true;
        }
    }

    void CompleteCurrentButton()
    {
        Destroy(ChosenButtonObjects[0]);
        ChosenButtonObjects.RemoveAt(0);
        ButtonUi.RemoveAt(0);
        ChosenButtons.RemoveAt(0);
        ActivateBools();

        for (int i = 0; i < buttonNames.Count; i++)
        {
            if (!animationScript.TrickBools[i])
            {
                animationScript.TrickBools[i] = true;
                return;
            }
        }
    }

    public void OnPerformButtonSouth(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (buttonBools[2])
            {
                CompleteCurrentButton();
            }
            else
            {
                animationScript.canPerformTrick = false;
                if (ButtonUi.Count > 2) ButtonUi[2].color = Color.red;
            }
        }
    }

    public void OnPerformButtonNorth(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (buttonBools[0])
            {
                CompleteCurrentButton();
            }
            else
            {
                animationScript.canPerformTrick = false;
                if (ButtonUi.Count > 0) ButtonUi[0].color = Color.red;
            }
        }
    }

    public void OnPerformButtonWest(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (buttonBools[3])
            {
                CompleteCurrentButton();
            }
            else
            {
                animationScript.canPerformTrick = false;
                if (ButtonUi.Count > 3) ButtonUi[3].color = Color.red;
            }
        }
    }

    public void OnPerformButtonEast(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (buttonBools[1])
            {
                CompleteCurrentButton();
            }
            else
            {
                animationScript.canPerformTrick = false;
                if (ButtonUi.Count > 1) ButtonUi[1].color = Color.red;
            }
        }
    }
}