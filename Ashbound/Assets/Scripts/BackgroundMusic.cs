
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Header("Background Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.5f;

    private void Start()
    {
        if (musicSource == null || backgroundMusic == null)
        {
            Debug.LogWarning("Background music or Audio Source is missing!");
            return;
        }

        musicSource.clip = backgroundMusic;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}
