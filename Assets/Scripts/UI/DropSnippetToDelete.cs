using UnityEngine;
using UnityEngine.EventSystems;

public class DropSnippetToDelete :MonoBehaviour, IDropHandler {

    private void Start()
    {
        SnippetsManagerUI.Instance.OnEnableDropArea += SnippetManagerUI_OnEnableDropArea;
        SnippetsManagerUI.Instance.OnDisableDropArea += SnippetManagerUI_OnDisableDropArea;
        gameObject.SetActive(false);
    }

    private void SnippetManagerUI_OnEnableDropArea()
    {
        gameObject.SetActive(true);
    }
    private void SnippetManagerUI_OnDisableDropArea()
    {
        gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<SnippetDragArea>().GetParentSnippet().GetComponent<StartSnippet>() == null)
        {
            Transform snippetObjToDelete = eventData.pointerDrag.GetComponent<SnippetDragArea>().GetParentSnippet();
            snippetObjToDelete.GetComponent<SnippetUI>().DeleteSnippet();
            
            SoundManager.Instance.PlaySnippetDeleteSound();
        }
    }
    private void OnDestroy()
    {
        SnippetsManagerUI.Instance.OnEnableDropArea -= SnippetManagerUI_OnEnableDropArea;
        SnippetsManagerUI.Instance.OnDisableDropArea -= SnippetManagerUI_OnDisableDropArea;
    }
}
