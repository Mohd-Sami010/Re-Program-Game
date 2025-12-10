using UnityEngine;

public class PickableItem :MonoBehaviour {

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public void PickUp(Transform itemHoldingPointTransform)
    {
        transform.SetParent(itemHoldingPointTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
    public void Drop()
    {
        transform.SetParent(null);
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }
    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    private void Start()
    {
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameRestart += GameManager_OnGameStop;
    }
    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        ResetItem();
    }
    private void ResetItem()
    {
        transform.SetParent(null);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
