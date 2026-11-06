using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectScale : MonoBehaviour,
    ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float selectedScale = 1.2f;
    [SerializeField] private float lerpSpeed = 12f;

    private Vector3 baseScale;
    private Vector3 targetScale;

    private void Awake()
    {
        baseScale = transform.localScale;
        targetScale = baseScale;
    }

    private void OnEnable()
    {
        transform.localScale = baseScale;
        targetScale = baseScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale, targetScale, Time.unscaledDeltaTime * lerpSpeed);
    }

    public void OnSelect(BaseEventData eventData)
    {
        targetScale = baseScale * selectedScale;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        targetScale = baseScale;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}