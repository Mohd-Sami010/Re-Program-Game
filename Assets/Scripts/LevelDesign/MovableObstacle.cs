using System.Collections;
using UnityEngine;

public class MovableObstacle :MonoBehaviour {

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    [SerializeField] private Vector3 destinedPosition;
    [SerializeField] private Quaternion destinedRotation;

    [SerializeField] private GameObject lightsObject;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    private void Start()
    {
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop += GameManager_OnGameRestart;
    }
    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        ResetObstacle();
    }

    public void RemoveObstacle()
    {
        StopAllCoroutines();
        StartCoroutine(MoveObstacle(destinedPosition, destinedRotation));
    }
    public void MoveObstacleToInitialPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveObstacle(initialPosition, initialRotation));
    }
    private void ResetObstacle()
    {
        StopAllCoroutines();
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        lightsObject.SetActive(false);
    }
    private IEnumerator MoveObstacle(Vector3 destinedPosition, Quaternion destinedRotation)
    {
        float duration = 1f;
        float timer = 0;
        lightsObject.SetActive(true);
        while (timer <  duration)
        {
            transform.position = Vector3.Lerp(transform.position, destinedPosition, timer / duration);
            transform.rotation = Quaternion.Lerp(transform.rotation, destinedRotation, timer/duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = destinedPosition;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameRestart -= GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop -= GameManager_OnGameRestart;
        StopAllCoroutines();
    }
}
