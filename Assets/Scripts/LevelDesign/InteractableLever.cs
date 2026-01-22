using UnityEngine;

public class InteractableLever :MonoBehaviour {

    private Animator leverAnimator;
    [SerializeField] private GameObject greenLightsObject;
    [SerializeField] private MovableObstacle obstacleToMove;
    [SerializeField] private bool isActivated = false;
    [Space]
    [SerializeField] private SpriteRenderer leverColourIndicatorSprite;
    private void Awake()
    {
        leverAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameRestart += GameManager_OnGameStop;

        obstacleToMove.SetColour(leverColourIndicatorSprite.color);
    }

    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        leverAnimator.SetBool("Activated", false);
        TurnOffLights();
        isActivated = false;
        GetComponent<AudioSource>().Stop();
    }

    public void Interact()
    {
        if (!isActivated)
        {
            leverAnimator.SetBool("Activated", true);
            TurnOnLights();
            obstacleToMove.RemoveObstacle();
            isActivated = true;
        }
        else
        {
            leverAnimator.SetBool("Activated", false);
            TurnOffLights();
            obstacleToMove.MoveObstacleToInitialPosition();
            isActivated = false;
        }
        PlaySound();
    }
    private void TurnOnLights()
    {
        greenLightsObject.SetActive(true);
    }
    private void TurnOffLights()
    {
        greenLightsObject.SetActive(false);
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
