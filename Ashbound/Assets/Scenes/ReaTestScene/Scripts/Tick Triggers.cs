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
    [SerializeField]
    private GameObject _player;
    private Animator _playerAnimator;
    private bool _trickCompleted;
    [SerializeField]
    private UIManager _manager;

    private void Start()
    {
        _manager = FindFirstObjectByType<UIManager>();
    }
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
        _trickCompleted = true;

    }

    public void FailTrick()
    {
        ResumeTime();
        for (int i = 0; i < Trickbools.Count; i++)
        {
            Trickbools[i] = false;
        }
        canRecordButtons = false;
        _playerAnimator.SetBool("Fail", true);
        StartCoroutine(SlowDownPlayer());
        if (_manager != null)
        {
            _manager.ShowRetryPanel();
        }
    }

    IEnumerator SlowDownPlayer()
    {
        PlayerController _playerScript = _player.GetComponent<PlayerController>();
        _playerScript.speed = 3;
        yield return new WaitForSeconds(1f);
        _playerScript.speed = 0;
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
            _player = other.gameObject;
            _playerAnimator = _player.GetComponent<Animator>();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_trickCompleted)
            {
                StartCoroutine(PerformTrick());
            }
            else
            {
                FailTrick();
            }
        }
    }

    IEnumerator PerformTrick()
    {
        if (_player != null)
        {
            if (_playerAnimator != null)
            {
                _playerAnimator.SetBool(Trick, true);
            }
        }
        yield return new WaitForSeconds(trickWaitTime);

        _playerAnimator.SetBool(Trick, false);
    }


}
