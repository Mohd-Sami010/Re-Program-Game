using UnityEngine;
using UnityEngine.EventSystems;

public class DropSnippetToDelete :MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<SnippetDragArea>().GetParentSnippet().GetComponent<StartSnippet>() == null)
        {
            Transform snippetObjToDelete = eventData.pointerDrag.GetComponent<SnippetDragArea>().GetParentSnippet();
            snippetObjToDelete.GetComponent<SnippetUI>().DeleteSnippet();
            
            SoundManager.Instance.PlaySnippetDeleteSound();
        }
    }
}
