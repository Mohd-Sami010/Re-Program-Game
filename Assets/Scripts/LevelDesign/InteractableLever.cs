using UnityEngine;

public class InteractableLever :MonoBehaviour {

    private Animator leverAnimator;
    [SerializeField] private GameObject[] greenLightsObjects;
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
        TurnOffLights();
    }

    public void Interact()
    {
        leverAnimator.SetBool("Activated", true);
        TurnOnLights();
        obstacleToMove.RemoveObstacle();
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
