using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuSnippetsDragArea : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler {
    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private RectTransform parentRectTransform;
    private Canvas canvas;

    private void Awake()
    {
        canvasGroup.alpha = 1f;
    }
    private void Start()
    {
        parentRectTransform = FindFirstObjectByType<MainMenuUI>().GetComponent<RectTransform>();
        canvas = FindFirstObjectByType<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        SoundManager.Instance.PlaySnippetGrabSound();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float wrapperScale = parentRectTransform.localScale.x;
        dragRectTransform.anchoredPosition += eventData.delta / (canvas.scaleFactor * wrapperScale);

        ClampToParent();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        SoundManager.Instance.PlaySnippetDropSound();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransform.SetAsLastSibling();
    }
    private void ClampToParent()
    {
        if (parentRectTransform == null) return;

        Vector3[] parentCorners = new Vector3[4];
        parentRectTransform.GetLocalCorners(parentCorners);

        Vector3[] myCorners = new Vector3[4];
        dragRectTransform.GetLocalCorners(myCorners);

        Vector2 newPos = dragRectTransform.anchoredPosition;

        float parentHalfWidth = (parentCorners[2].x - parentCorners[0].x) / 2f;
        float parentHalfHeight = (parentCorners[2].y - parentCorners[0].y) / 2f;

        float myHalfWidth = (myCorners[2].x - myCorners[0].x) / 2f;
        float myHalfHeight = (myCorners[2].y - myCorners[0].y) / 2f;

        newPos.x = Mathf.Clamp(newPos.x, -parentHalfWidth + myHalfWidth, parentHalfWidth - myHalfWidth);
        newPos.y = Mathf.Clamp(newPos.y, -parentHalfHeight + myHalfHeight, parentHalfHeight - myHalfHeight);

        dragRectTransform.anchoredPosition = newPos;
    }
}
