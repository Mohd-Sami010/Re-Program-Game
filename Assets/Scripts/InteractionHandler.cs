using UnityEngine;

public class InteractionHandler :MonoBehaviour {

    [SerializeField] private float interactRange = 0.7f;
    [SerializeField] private Transform itemHoldingPointTransform;
    [SerializeField] private bool isHoldingItem = false;

    public void Interact()
    {
        if (isHoldingItem)
        {
            // Drop Item
            PickableItem heldItem = itemHoldingPointTransform.GetComponentInChildren<PickableItem>();
            if (heldItem != null)
            {
                heldItem.Drop();
            }
            isHoldingItem = false;
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out InteractableLever lever))
            {
                lever.Interact();
            }
            else if (collider.TryGetComponent(out PickableItem item))
            {
                item.PickUp(itemHoldingPointTransform);
                isHoldingItem = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
