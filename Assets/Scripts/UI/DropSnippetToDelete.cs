using UnityEngine;
using UnityEngine.EventSystems;

public class DropSnippetToDelete :MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<SnippetDragArea>().GetParentSnippet().GetComponent<StartSnippet>() == null)
        {
            Destroy(eventData.pointerDrag.GetComponent<SnippetDragArea>().GetParentSnippet().gameObject);
            SnippetsManagerUI.Instance.UpdateSnippetUIsList();
            SoundManager.Instance.PlaySnippetDeleteSound();
        }
    }
}
