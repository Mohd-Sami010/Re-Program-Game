using UnityEngine;

public class InteractionHandler :MonoBehaviour {

    [SerializeField] private float interactRange = 0.7f;

    public void Interact()
    {
        // Check Interaction
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out InteractableLever lever))
            {
                lever.Interact();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
