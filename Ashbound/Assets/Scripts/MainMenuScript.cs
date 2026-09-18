using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [Tooltip("Exact name of the campaign scene as it appears in Build Settings.")]
    [SerializeField] private string campaignSceneName = "Campaign";

    [Tooltip("Exact name of the endless runner scene as it appears in Build Settings.")]
    [SerializeField] private string endlessRunnerSceneName = "EndlessRunner";

    [Header("Audio Sources")]
    [Tooltip("Loops the background music. Set 'Loop' on the AudioSource itself too.")]
    [SerializeField] private AudioSource musicSource;

    [Tooltip("Plays one-shot UI sound effects (clicks, hovers, etc).")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonClickSfx;
    [SerializeField] private AudioClip buttonHoverSfx;

    [Header("Settings")]
    [Range(0f, 1f)][SerializeField] private float musicVolume = 0.6f;
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void OnPlayCampaignClicked()
    {
        PlayClickSfx();
        LoadScene(campaignSceneName);
    }

    public void OnPlayEndlessRunnerClicked()
    {
        PlayClickSfx();
        LoadScene(endlessRunnerSceneName);
    }

    public void OnQuitClicked()
    {
        PlayClickSfx();
        QuitGame();
    }

    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("MainMenuManager: Scene name is empty — check the Inspector fields.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void PlayBackgroundMusic()
    {
        if (musicSource == null || backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    private void PlayClickSfx()
    {
        if (sfxSource == null || buttonClickSfx == null) return;
        sfxSource.PlayOneShot(buttonClickSfx, sfxVolume);
    }

    public void PlayHoverSfx()
    {
        if (sfxSource == null || buttonHoverSfx == null) return;
        sfxSource.PlayOneShot(buttonHoverSfx, sfxVolume);
    }
}