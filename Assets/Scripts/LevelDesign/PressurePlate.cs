using UnityEngine;

public class PressurePlate :MonoBehaviour {

    [SerializeField] private GameObject greenLightsObject;
    [SerializeField] private MovableObstacle obstacleToMove;
    [Space]
    [SerializeField] private SpriteRenderer leverColourIndicatorSprite;

    private Animator animator;
    private int objectsOnPlate = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        obstacleToMove.SetColour(leverColourIndicatorSprite.color);
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
    private void OnDrawGizmos()
    {
        if (obstacleToMove == null) return;
        Gizmos.color = leverColourIndicatorSprite.color;
        Gizmos.DrawLine(transform.position, obstacleToMove.transform.position);
    }
}
