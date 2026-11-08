using System.Collections;
using UnityEngine;

public class ActionAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip Breathing, SideMove;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBreathing()
    {
        audioSource.clip = Breathing;
        audioSource.Play();
        audioSource.loop = true;

    }

    public void PlaySideMove()
    {
        audioSource.clip = SideMove;
        audioSource.Play();
    }

    public IEnumerator playSideAudio()
    {
        PlaySideMove();
        yield return new WaitForSeconds(0.5f);
        PlayBreathing();
    }


}
