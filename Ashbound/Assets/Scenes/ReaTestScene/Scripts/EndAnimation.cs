using UnityEngine;

public class EndAnimation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animator _playerAnimator = other.GetComponent<Animator>();
            _playerAnimator.SetBool("End", true);
        }
    }
}
