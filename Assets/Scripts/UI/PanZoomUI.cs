using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.UI;

public class PanZoomUI :MonoBehaviour, IDragHandler, IScrollHandler {

    private RectTransform target;
    private RectTransform contentWrapperRectTransform;
    [SerializeField] private RectTransform uIContainerRectTransform;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2.5f;

    [Header("Pan Settings")]
    [SerializeField] private float dragSpeed = 1f;

    private void Awake()
    {
        target = GetComponent<RectTransform>();
        contentWrapperRectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        SnippetsManagerUI.Instance.OnEnableDropArea += SnippetsManagerUI_OnEnableDropArea;
        SnippetsManagerUI.Instance.OnDisableDropArea += SnippetsManagerUI_OnDisableDropArea;
    }
    private void SnippetsManagerUI_OnEnableDropArea()
    {
        GetComponent<Image>().enabled = false;
    }
    private void SnippetsManagerUI_OnDisableDropArea()
    {
        GetComponent<Image>().enabled = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        target.anchoredPosition += eventData.delta * dragSpeed;
    }
    private void Update()
    {
        HandlePinchZoom();
        ClampPosition();
    }
    void HandlePinchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            // distance between touches (current & previous)
            float prevDist = (t0.position - t0.deltaPosition - (t1.position - t1.deltaPosition)).magnitude;
            float currDist = (t0.position - t1.position).magnitude;

            float diff = currDist - prevDist;

            float zoomSpeed = 0.01f;

            float scale = contentWrapperRectTransform.localScale.x + diff * zoomSpeed;

            scale = Mathf.Clamp(scale, minScale, maxScale);

            contentWrapperRectTransform.localScale = new Vector3(scale, scale, 1);
        }
    }
    void ClampPosition()
    {
        RectTransform wrapper = contentWrapperRectTransform;
        RectTransform mask = uIContainerRectTransform;     // the window

        Vector3 pos = wrapper.anchoredPosition;

        float scaledWidth = wrapper.rect.width * wrapper.localScale.x;
        float scaledHeight = wrapper.rect.height * wrapper.localScale.y;

        float maskWidth = mask.rect.width;
        float maskHeight = mask.rect.height;

        float limitX = Mathf.Max((scaledWidth - maskWidth) / 2f, 0f);
        float limitY = Mathf.Max((scaledHeight - maskHeight) / 2f, 0f);

        pos.x = Mathf.Clamp(pos.x, -limitX, limitX);
        pos.y = Mathf.Clamp(pos.y, -limitY, limitY);

        wrapper.anchoredPosition = pos;
    }

    public void OnScroll(PointerEventData eventData)
    {
        float s = target.localScale.x;
        s += (eventData.scrollDelta.y * zoomSpeed);
        s = Mathf.Clamp(s, minScale, maxScale);
        target.localScale = Vector3.one * s;
    }
    public void Zoom(float zoomAmount)
    {
        //StartCoroutine(ZoomSmoothly(zoomAmount));
        float s = target.localScale.x;
        s += zoomAmount;
        s = Mathf.Clamp(s, minScale, maxScale);
        target.localScale = Vector3.one * s;
    }
    private IEnumerator ZoomSmoothly(float zoomAmount) { 
        float duration = 0.15f;
        float timer = 0;
        float targetScale = target.localScale.x;
        while (duration > timer)
        {
            float scale = Mathf.Lerp(target.localScale.x, targetScale + zoomAmount, timer / duration);
            scale = Mathf.Clamp(scale, minScale, maxScale);
            target.localScale = Vector3.one * scale;

            timer += Time.deltaTime;
            yield return null;
        }
        float s = targetScale;
        s += zoomAmount;
        s = Mathf.Clamp(s, minScale, maxScale);
        target.localScale = Vector3.one * s;
    }
    private void OnDestroy()
    {
        SnippetsManagerUI.Instance.OnEnableDropArea -= SnippetsManagerUI_OnEnableDropArea;
        SnippetsManagerUI.Instance.OnDisableDropArea -= SnippetsManagerUI_OnDisableDropArea;
    }
}
