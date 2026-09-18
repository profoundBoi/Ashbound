using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [Tooltip("Exact name of the campaign scene as it appears in Build Settings.")]
    [SerializeField] private string campaignSceneName = "Campaign";

    [Tooltip("Exact name of the endless runner scene as it appears in Build Settings.")]
    [SerializeField] private string endlessRunnerSceneName = "EndlessRunner";

    [Header("Audio Sources (each on its own GameObject)")]
    [Tooltip("Drag the 'Music' GameObject here — its AudioSource is grabbed automatically.")]
    [SerializeField] private GameObject musicObject;

    [Tooltip("Drag the 'ClickSfx' GameObject here — its AudioSource is grabbed automatically.")]
    [SerializeField] private GameObject clickSfxObject;

    [Tooltip("Drag the 'SelectSfx' GameObject here — its AudioSource is grabbed automatically.")]
    [SerializeField] private GameObject selectSfxObject;

    private AudioSource musicSource;
    private AudioSource clickSfxSource;
    private AudioSource selectSfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonClickSfx;
    [SerializeField] private AudioClip buttonSelectSfx;

    [Header("Settings")]
    [Range(0f, 1f)][SerializeField] private float musicVolume = 0.6f;
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;

    [Header("Controller Support")]
    [Tooltip("The button/UI element that should be highlighted by default so gamepad/keyboard navigation works immediately.")]
    [SerializeField] private GameObject firstSelectedButton;

    private void Awake()
    {
        CacheAudioSources();
    }

    private void CacheAudioSources()
    {
        if (musicObject != null) musicSource = musicObject.GetComponent<AudioSource>();
        if (clickSfxObject != null) clickSfxSource = clickSfxObject.GetComponent<AudioSource>();
        if (selectSfxObject != null) selectSfxSource = selectSfxObject.GetComponent<AudioSource>();

        if (musicObject != null && musicSource == null)
            Debug.LogWarning("MainMenuManager: musicObject has no AudioSource component.");
        if (clickSfxObject != null && clickSfxSource == null)
            Debug.LogWarning("MainMenuManager: clickSfxObject has no AudioSource component.");
        if (selectSfxObject != null && selectSfxSource == null)
            Debug.LogWarning("MainMenuManager: selectSfxObject has no AudioSource component.");
    }

    private void Start()
    {
        PlayBackgroundMusic();
        SelectFirstButton();
    }

    private void SelectFirstButton()
    {
        if (firstSelectedButton == null || EventSystem.current == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
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
        if (clickSfxSource == null || buttonClickSfx == null) return;
        clickSfxSource.PlayOneShot(buttonClickSfx, sfxVolume);
    }

    public void PlaySelectSfx()
    {
        if (selectSfxSource == null || buttonSelectSfx == null) return;
        selectSfxSource.PlayOneShot(buttonSelectSfx, sfxVolume);
    }
}