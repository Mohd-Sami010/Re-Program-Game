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
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameRestart += GameManager_OnGameStop;
    }
    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        greenLightsObject.SetActive(false);
        animator.SetBool("Pressed", false);
        objectsOnPlate = 0;
        //obstacleToMove.MoveObstacleToInitialPosition();
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().volume =0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PickableItem>() != null || collision.GetComponent<RobotController>())
        {
            if (obstacleToMove == null) return;
            greenLightsObject.SetActive(true);
            obstacleToMove.RemoveObstacle();
            animator.SetBool("Pressed", true);
            objectsOnPlate++;
            if (objectsOnPlate > 1) return;
            GetComponent<AudioSource>().volume = 1;
            PlaySound();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PickableItem>() != null || collision.GetComponent<RobotController>())
        {
            objectsOnPlate--;
            if (obstacleToMove == null || objectsOnPlate > 0 ||
                GameManager.Instance.GetCurrentGameState() == GameManager.GameState.GameOver) return;
            greenLightsObject.SetActive(false);
            obstacleToMove.MoveObstacleToInitialPosition();
            animator.SetBool("Pressed", false);
            PlaySound();
        }
    }
    private void OnDrawGizmos()
    {
        if (obstacleToMove == null) return;
        Gizmos.color = leverColourIndicatorSprite.color;
        Gizmos.DrawLine(transform.position, obstacleToMove.transform.position);
    }
    private void PlaySound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStop -= GameManager_OnGameStop;
        GameManager.Instance.OnGameRestart -= GameManager_OnGameStop;
    }
}
