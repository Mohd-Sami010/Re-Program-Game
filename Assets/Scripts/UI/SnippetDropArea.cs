using UnityEngine;
using UnityEngine.EventSystems;
public class SnippetDropArea :MonoBehaviour, IDropHandler {
    private bool haveSnippet = false;
    [SerializeField] private SnippetUI parentSnippet;
    [SerializeField] private GameObject imageObject;

    private void Start()
    {
        SnippetsManagerUI snippetsManagerUI = SnippetsManagerUI.Instance;

        snippetsManagerUI.OnEnableDropArea += SnippetsManagerUI_OnEnableDropArea;
        snippetsManagerUI.OnDisableDropArea += SnippetsManagerUI_OnDisableDropArea;

        imageObject.SetActive(false);
    }

    private void SnippetsManagerUI_OnDisableDropArea()
    {
        imageObject.SetActive(false);
    }

    private void SnippetsManagerUI_OnEnableDropArea()
    {
        imageObject.SetActive(true);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && !haveSnippet)
        {
            RectTransform snippetRect = eventData.pointerDrag
                .GetComponent<SnippetDragArea>()
                .GetParentSnippet()
                .GetComponent<RectTransform>();

            if (snippetRect.GetComponent<StartSnippet>() != null) return;

            snippetRect.SetParent(transform, false);
            snippetRect.anchoredPosition = Vector2.zero;

            // Force perfect LEFT alignment under parent
            Vector2 newPos = snippetRect.anchoredPosition;
            newPos.x = -((RectTransform)transform).rect.width / 2f + snippetRect.rect.width / 2f;
            snippetRect.anchoredPosition = newPos;


            haveSnippet = true;

            parentSnippet.SetNextSnippetUI(snippetRect.GetComponent<SnippetUI>());
            SnippetsManagerUI.Instance.UpdateSnippetUIsList();
            SoundManager.Instance.PlayUISound2();
        }
    }
    public void RemoveSnippet()
    {
        haveSnippet = false;
        parentSnippet.SetNextSnippetUI(null);
        SnippetsManagerUI.Instance.UpdateSnippetUIsList();
    }
    private void OnDestroy()
    {
        SnippetsManagerUI snippetsManagerUI = SnippetsManagerUI.Instance;

        snippetsManagerUI.OnEnableDropArea -= SnippetsManagerUI_OnEnableDropArea;
        snippetsManagerUI.OnDisableDropArea -= SnippetsManagerUI_OnDisableDropArea;
    }
}
