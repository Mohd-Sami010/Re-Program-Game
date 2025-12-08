using UnityEngine;

public class PressurePlate :MonoBehaviour {

    [SerializeField] private GameObject greenLightsObject;
    [SerializeField] private MovableObstacle obstacleToMove;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PickableItem pickableItem))
        {
            greenLightsObject.SetActive(true);
            obstacleToMove.RemoveObstacle();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PickableItem pickableItem))
        {
            if (obstacleToMove == null) return;
            greenLightsObject.SetActive(false);
            obstacleToMove.MoveObstacleToInitialPosition();
        }
    }

}
