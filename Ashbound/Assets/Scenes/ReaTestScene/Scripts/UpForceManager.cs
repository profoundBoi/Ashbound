using System.Collections;
using UnityEngine;

public class UpForceManager : MonoBehaviour
{
    public float upForceAmount = 10f;
    private GameObject _Player;
    [SerializeField]
    private float _startTime, _waitTime, _swingSpeed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _Player = other.gameObject;
            Rigidbody rb = _Player.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * upForceAmount, ForceMode.Impulse);
            StartCoroutine(SwingSpeedManager());
        }
    }

    IEnumerator SwingSpeedManager()
    {

        PlayerController _playerScript = _Player.GetComponent<PlayerController>();
        float _currentSpeed = _playerScript.speed;
        yield return new WaitForSeconds(_startTime);
        if (_playerScript != null)
        {
            _playerScript.speed = _swingSpeed;
        }
        yield return new WaitForSeconds(_waitTime);
        if (_playerScript != null)
        {
            _playerScript.speed = _currentSpeed;
        }
    }
}
