using UnityEngine;

public class PressurePlate :MonoBehaviour {

    [SerializeField] private GameObject greenLightsObject;
    [SerializeField] private MovableObstacle obstacleToMove;
    private Animator animator;
    private int objectsOnPlate = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PickableItem>() != null || collision.GetComponent<RobotController>())
        {
            greenLightsObject.SetActive(true);
            obstacleToMove.RemoveObstacle();
            animator.SetBool("Pressed", true);
            objectsOnPlate++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PickableItem>() != null || collision.GetComponent<RobotController>())
        {
            objectsOnPlate--;
            if (obstacleToMove == null || objectsOnPlate > 0) return;
            greenLightsObject.SetActive(false);
            obstacleToMove.MoveObstacleToInitialPosition();
            animator.SetBool("Pressed", false);
        }
    }

}
