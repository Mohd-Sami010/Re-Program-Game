using UnityEngine;

public class InteractableLever :MonoBehaviour {

    private Animator leverAnimator;
    [SerializeField] private GameObject greenLightsObject;
    [SerializeField] private MovableObstacle obstacleToMove;

    private void Awake()
    {
        leverAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameRestart += GameManager_OnGameStop;
    }

    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        leverAnimator.SetBool("Activated", false);
        greenLightsObject.SetActive(false);
    }

    public void Interact()
    {
        leverAnimator.SetBool("Activated", true);
        greenLightsObject.SetActive(true);
        obstacleToMove.RemoveObstacle();
    }

}
