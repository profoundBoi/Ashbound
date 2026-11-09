using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldButton : Selectable, ISubmitHandler, IPointerClickHandler
{
    public UnityEvent onClick;
    public UnityEvent onSelected;

    [SerializeField] Transform visual;  
    [SerializeField] float selectedScale = 1.15f;
    [SerializeField] float pressedScale = 0.9f;
    [SerializeField] float lerpSpeed = 15f;
    [SerializeField] float pressDuration = 0.1f;

    Vector3 baseScale;
    bool isSelected;
    float pressTimer;

    protected override void Awake()
    {
        base.Awake();
        transition = Transition.None;         
        if (visual == null) visual = transform;
        baseScale = visual.localScale;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        isSelected = false;
        if (visual != null) visual.localScale = baseScale;
    }

    void Update()
    {
        float target = pressTimer > 0f ? pressedScale : (isSelected ? selectedScale : 1f);
        pressTimer -= Time.unscaledDeltaTime;
        visual.localScale = Vector3.Lerp(visual.localScale, baseScale * target,
                                         Time.unscaledDeltaTime * lerpSpeed);
    }

    public override void OnSelect(BaseEventData e)
    {
        base.OnSelect(e);
        isSelected = true;
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
}