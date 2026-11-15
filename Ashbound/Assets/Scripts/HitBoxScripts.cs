using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HitBoxScripts : MonoBehaviour
{
    [SerializeField]
    private bool isTripFail, isHitFail;
    public GameObject _player;
    private UIManager _manager;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _manager = FindAnyObjectByType<UIManager>();
    }
    IEnumerator SlowDownPlayer()
    {
        if (isHitFail)
        {
            PlayerController _playerScript = _player.GetComponent<PlayerController>();
            Animator _playerAnimator = _player.GetComponent<Animator>();
            _playerAnimator.SetBool("Fail", true);
            _playerAnimator.SetBool("WallHit", true);
            yield return new WaitForSeconds(0f);
            _playerScript.speed = 0;
            _manager.ShowRetryPanel();

        }
        else if (isTripFail)
        {
            PlayerController _playerScript = _player.GetComponent<PlayerController>();
            Animator _playerAnimator = _player.GetComponent<Animator>();
            _playerAnimator.SetBool("Fail", true);
            _playerAnimator.SetBool("SoccerTrip", true);
            _playerScript.speed = 3;
            yield return new WaitForSeconds(1f);
            _playerScript.speed = 0;
            _manager.ShowRetryPanel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SlowDownPlayer());
        }
    }
}
