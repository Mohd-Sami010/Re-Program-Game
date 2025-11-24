using UnityEngine;
using UnityEngine.EventSystems;

public class SnippetDragArea :MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    
    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    //[SerializeField] private CanvasGroup dragAreaCanvasGroup;

    private RectTransform parentRectTransform;
    private Canvas canvas;

    private void Awake()
    {
        canvasGroup.alpha = 1f;
    }
    private void Start()
    {
        parentRectTransform = SnippetsManagerUI.Instance.GetParentContainer().GetComponent<RectTransform>();
        canvas = FindFirstObjectByType<Canvas>();

        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart; ;
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
    }
    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }
    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        SnippetsManagerUI.Instance.EnableDropArea();
        SoundManager.Instance.PlaySnippetGrabSound();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float wrapperScale = parentRectTransform.localScale.x;
        dragRectTransform.anchoredPosition += eventData.delta / (canvas.scaleFactor * wrapperScale);

        if (dragRectTransform.parent != parentRectTransform && dragRectTransform.parent.GetComponent<SnippetDropArea>() != null)
        {
            dragRectTransform.transform.parent.GetComponent<SnippetDropArea>().RemoveSnippet();
            dragRectTransform.SetParent(parentRectTransform);
            dragRectTransform.localScale = Vector3.one;
        }
        ClampToParent();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        SoundManager.Instance.PlaySnippetDropSound();
        SnippetsManagerUI.Instance.DisableDropArea();
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
    public Transform GetParentSnippet()
    {
        return dragRectTransform.transform;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameRestart -= GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop -= GameManager_OnGameStop;
    }
}
