using UnityEngine;
using UnityEngine.UI;

public class CodeEditorUiFullScreen :MonoBehaviour {

    [SerializeField] private RectTransform uiContainerRectTransform;
    [SerializeField] private PanZoomUI panZoomUI;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button fullScreenButton;
    [SerializeField] private Button minimizeScreenButton;

    [Space]
    [SerializeField] private float zoomAmount = 0.15f;

    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalOffsetMin;
    private Vector2 originalOffsetMax;

    private bool isFullScreen = false;

    private void Start()
    {
        fullScreenButton.onClick.AddListener(() => {
            EnterFullScreen();
            SoundManager.Instance.PlayUISound1();
        });
        minimizeScreenButton.onClick.AddListener(() => {
            ExitFullScreen();
            SoundManager.Instance.PlayUISound1();
        });

        minimizeScreenButton.gameObject.SetActive(false);
    }
    private void EnterFullScreen()
    {
        if (isFullScreen) return;
        isFullScreen = true;

        originalAnchorMin = uiContainerRectTransform.anchorMin;
        originalAnchorMax = uiContainerRectTransform.anchorMax;
        originalOffsetMin = uiContainerRectTransform.offsetMin;
        originalOffsetMax = uiContainerRectTransform.offsetMax;

        uiContainerRectTransform.anchorMin = Vector2.zero;
        uiContainerRectTransform.anchorMax = Vector2.one;
        uiContainerRectTransform.offsetMin = Vector2.zero;
        uiContainerRectTransform.offsetMax = Vector2.zero;

        backgroundImage.color = new Color(
            backgroundImage.color.r,
            backgroundImage.color.g,
            backgroundImage.color.b,
            1f
        );
        panZoomUI.Zoom(zoomAmount);
        fullScreenButton.gameObject.SetActive(false);
        minimizeScreenButton.gameObject.SetActive(true);
    }
    private void ExitFullScreen()
    {
        if (!isFullScreen) return;
        isFullScreen = false;

        // Restore original rect
        uiContainerRectTransform.anchorMin = originalAnchorMin;
        uiContainerRectTransform.anchorMax = originalAnchorMax;
        uiContainerRectTransform.offsetMin = originalOffsetMin;
        uiContainerRectTransform.offsetMax = originalOffsetMax;

        // Restore background opacity
        backgroundImage.color = new Color(
            backgroundImage.color.r,
            backgroundImage.color.g,
            backgroundImage.color.b,
            0.9f
        );
        panZoomUI.Zoom(zoomAmount);
        fullScreenButton.gameObject.SetActive(true);
        minimizeScreenButton.gameObject.SetActive(false);
    }
}
