using UnityEngine;

public class FootSteps : MonoBehaviour
{
    private AudioSource _audio;
    [SerializeField]
    private AudioClip FootStep, Tirebounce;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            _audio.clip = FootStep;
            _audio.Play();
        }
        else if (other.CompareTag("Tire"))
        {
            _audio.clip = Tirebounce;
            _audio.Play();
        }
    }
}
