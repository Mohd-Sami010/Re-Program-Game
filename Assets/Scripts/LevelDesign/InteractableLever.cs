using UnityEngine;

public class InteractableLever :MonoBehaviour {

    private Animator leverAnimator;
    [SerializeField] private GameObject[] greenLightsObjects;
    [SerializeField] private MovableObstacle obstacleToMove;
    [SerializeField] private bool isActivated = false;
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
        TurnOffLights();
    }

    public void Interact()
    {
        if (isActivated)
        {
            leverAnimator.SetBool("Activated", true);
            TurnOnLights();
            obstacleToMove.RemoveObstacle();
        }
        else
        {
            leverAnimator.SetBool("Activated", false);
            TurnOffLights();
            obstacleToMove.MoveObstacleToInitialPosition();
        }
    }
    private void TurnOnLights()
    {
        foreach (GameObject lightObject in greenLightsObjects)
        {
            lightObject.SetActive(true);
        }
    }
    private void TurnOffLights()
    {
        foreach (GameObject lightObject in greenLightsObjects)
        {
            lightObject.SetActive(false);
        }
    }
}
