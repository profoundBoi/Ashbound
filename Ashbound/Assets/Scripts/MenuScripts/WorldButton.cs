using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldButton : Selectable, ISubmitHandler, IPointerClickHandler
{
    public UnityEvent onClick;
    public UnityEvent onSelected;

    [Header("Visual")]
    [SerializeField] Transform visual;
    [SerializeField] float lerpSpeed = 15f;

    [Header("Scale")]
    [SerializeField] float selectedScale = 1.15f;
    [SerializeField] float pressedScale = 0.9f;
    [SerializeField] float pressDuration = 0.1f;

    [Header("Colour")]
    [SerializeField] Color selectedColor = Color.yellow;
    [SerializeField] Color disabledColor = Color.gray;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;  
    [SerializeField] AudioClip selectSound;
    [SerializeField, Range(0f, 1f)] float selectVolume = 1f;

    Graphic graphic;
    SpriteRenderer sprite;

    Vector3 baseScale;
    Color baseColor;
    bool isSelected;
    float pressTimer;

    Color CurrentColor
    {
        get => graphic != null ? graphic.color : sprite != null ? sprite.color : Color.white;
        set
        {
            if (graphic != null) graphic.color = value;
            else if (sprite != null) sprite.color = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        transition = Transition.None;
        if (visual == null) visual = transform;
        baseScale = visual.localScale;

        graphic = visual.GetComponent<Graphic>();
        if (graphic == null) sprite = visual.GetComponent<SpriteRenderer>();
        baseColor = CurrentColor;

        if (Application.isPlaying && audioSource == null)
        {
            if (!TryGetComponent(out audioSource))
                audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;   
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        isSelected = false;
        if (visual != null)
        {
            visual.localScale = baseScale;
            CurrentColor = baseColor;
        }
    }

    void Update()
    {
        float target = pressTimer > 0f ? pressedScale : (isSelected ? selectedScale : 1f);
        pressTimer -= Time.unscaledDeltaTime;
        float t = Time.unscaledDeltaTime * lerpSpeed;

        visual.localScale = Vector3.Lerp(visual.localScale, baseScale * target, t);

        Color targetColor = !IsInteractable() ? disabledColor
                          : isSelected ? selectedColor
                          : baseColor;
        CurrentColor = Color.Lerp(CurrentColor, targetColor, t);
    }

    public override void OnSelect(BaseEventData e)
    {
        base.OnSelect(e);
        isSelected = true;
        PlaySelectSound();
        onSelected.Invoke();
    }

    public override void OnDeselect(BaseEventData e)
    {
        base.OnDeselect(e);
        isSelected = false;
    }

    public override void OnPointerEnter(PointerEventData e)
    {
        base.OnPointerEnter(e);
        if (IsInteractable()) Select();
    }

    public void OnSubmit(BaseEventData e) => Press();
    public void OnPointerClick(PointerEventData e) => Press();

    void Press()
    {
        if (!IsInteractable()) return;
        pressTimer = pressDuration;
        onClick.Invoke();
    }

    void PlaySelectSound()
    {
        if (selectSound != null && audioSource != null)
            audioSource.PlayOneShot(selectSound, selectVolume);
    }
}