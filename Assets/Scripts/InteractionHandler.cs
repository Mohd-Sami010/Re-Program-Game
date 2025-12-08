using UnityEngine;

public class InteractionHandler :MonoBehaviour {

    [SerializeField] private float interactRange = 0.7f;
    [SerializeField] private Transform itemHoldingPointTransform;

    public enum InteractionType
    {
        Lever,
        PickItem,
        DropItem,
    }
    private void Start()
    {
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameRestart += GameManager_OnGameStop;
    }
    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        if (RobotController.Instance.IsHoldingItem())
        {
            // Drop Item
            PickableItem heldItem = itemHoldingPointTransform.GetComponentInChildren<PickableItem>();
            if (heldItem != null)
            {
                heldItem.Drop();
            }
            RobotController.Instance.SetIsHoldingItem(false);
        }
    }
    public void Interact()
    {
        if (RobotController.Instance.IsHoldingItem())
        {
            // Drop Item
            PickableItem heldItem = itemHoldingPointTransform.GetComponentInChildren<PickableItem>();
            if (heldItem != null)
            {
                heldItem.Drop();
            }
            RobotController.Instance.SetIsHoldingItem(false);
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
                RobotController.Instance.SetIsHoldingItem(true);
            }
        }
    }
    public InteractionType CheckInteractionType()
    {
        if (RobotController.Instance.IsHoldingItem())
        {
            return InteractionType.DropItem;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out InteractableLever lever))
            {
                return InteractionType.Lever;
            }
            else if (collider.TryGetComponent(out PickableItem item))
            {
                return InteractionType.PickItem;
            }
        }
        return InteractionType.Lever; // Default return value
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
