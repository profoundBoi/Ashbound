using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class TrickTriggers : MonoBehaviour
{
    [SerializeField]
    private string Trick;
    [SerializeField]
    private float _slowTime;
    public GameObject SlowTimePanel;
    public List<bool> Trickbools;
    public List<GameObject> _ButtonUi;
    private int _buttonIndex = 0;
    public bool canRecordButtons;
    [SerializeField]
    private List<int> BoolPosition;
    [SerializeField]
    private float trickWaitTime;
    private GameObject _player;
    private Animator _playerAnimator;

    void SlowDownTime()
    {
        SlowTimePanel.SetActive(true);
        Time.timeScale = _slowTime;
    }

    void ResumeTime()
    {
        SlowTimePanel.SetActive(false);
        Time.timeScale = 1;

    }

    void TrickComplete()
    {
        ResumeTime();
        RemoveAllUI();
        canRecordButtons = false;

    }

    public void FailTrick()
    {
        ResumeTime();
        for (int i = 0; i < Trickbools.Count; i++)
        {
            Trickbools[i] = false;
        }
        canRecordButtons = false;

    }

    public void MoveToNextButton()
    {
        if (_buttonIndex < BoolPosition.Count - 1)
        {
            _buttonIndex++;
            TrickActivation();
        }
        else if (_buttonIndex == BoolPosition.Count - 1)
        {
            TrickComplete();
        }
    }

    void TrickActivation()
    {

        for (int i = 0; i < Trickbools.Count; i++)
        {
            Trickbools[i] = false;

        }
        Trickbools[BoolPosition[_buttonIndex]] = true;

        for (int e = 0; e < _ButtonUi.Count; e++)
        {
            _ButtonUi[e].SetActive(false);

        }
        _ButtonUi[BoolPosition[_buttonIndex]].SetActive(true);

    }

    void RemoveAllUI()
    {
        for (int i = 0; i < _ButtonUi.Count; i++)
        {
            _ButtonUi[i].SetActive(false);

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canRecordButtons = true;
            SlowDownTime();
            TrickActivation();
            PlayerController _playerScript = other.gameObject.GetComponent<PlayerController>();
            _playerScript._trickTriggerScript = GetComponent<TrickTriggers>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player = other.gameObject;
            StartCoroutine(PerformTrick());
        }
    }

    IEnumerator PerformTrick()
    {
        if (_player != null)
        {
            _playerAnimator = _player.GetComponent<Animator>();
            if (_playerAnimator != null)
            {
                _playerAnimator.SetBool(Trick, true);
            }
        }
        yield return new WaitForSeconds(trickWaitTime);

        _playerAnimator.SetBool(Trick, false);
    }


}
