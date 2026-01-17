using System.Collections;
using UnityEngine;

public class MovableObstacle :MonoBehaviour {

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    [SerializeField] private Vector3 destinedPosition;
    [SerializeField] private Quaternion destinedRotation;

    [SerializeField] private GameObject lightsObject;
    [SerializeField] private SpriteRenderer colourSprite;

    [Header("Audio")]
    [SerializeField] private AudioSource movingSoundAudioSource;
    [SerializeField] private AudioSource movingStopSoundAudioSource;
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
        movingStopSoundAudioSource.Stop();
        movingSoundAudioSource.Stop();
        movingSoundAudioSource.volume = 0;
        movingStopSoundAudioSource.volume = 0;
    }

    public void RemoveObstacle()
    {
        StopAllCoroutines();
        StartCoroutine(MoveObstacle(destinedPosition, destinedRotation));
    }
    public void MoveObstacleToInitialPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveObstacle(initialPosition, initialRotation, false));
    }
    private void ResetObstacle()
    {
        StopAllCoroutines();
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        lightsObject.SetActive(false);
    }
    private IEnumerator MoveObstacle(Vector3 destinedPosition, Quaternion destinedRotation, bool lightsOn = true)
    {
        movingSoundAudioSource.volume = 1;
        movingStopSoundAudioSource.volume = 1;

        PlayMovingSound();
        movingStopSoundAudioSource.Stop();

        float duration = 1f;
        float timer = 0;

        if (lightsOn) lightsObject.SetActive(true);
        else lightsObject.SetActive(false);

        while (timer <  duration)
        {
            transform.position = Vector3.Lerp(transform.position, destinedPosition, timer / duration);
            transform.rotation = Quaternion.Lerp(transform.rotation, destinedRotation, timer/duration);
            timer += Time.deltaTime;
            if (timer / duration >= 0.55f && movingSoundAudioSource.isPlaying)
            {
                PlayMovingStopSound();
                movingSoundAudioSource.Stop();
            }
            yield return null;
        }
        movingSoundAudioSource.Stop();
        transform.position = destinedPosition;

    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameRestart -= GameManager_OnGameRestart;
        GameManager.Instance.OnGameStop -= GameManager_OnGameRestart;
        StopAllCoroutines();
    }
    private void OnDrawGizmos()
    {
        // --- Resolve start values in Edit Mode ---
        Vector3 startPos = Application.isPlaying ? initialPosition : transform.position;
        Quaternion startRot = Application.isPlaying ? initialRotation : transform.rotation;

        // --- Get visual size ---
        float rotationLineLength = 0.5f;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Bounds b = sr.bounds;
            rotationLineLength = Mathf.Max(b.size.x, b.size.y) * 0.5f;
        }

        // --- 1. Movement path ---
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPos, destinedPosition);

        // --- 2. Initial rotation (Red) ---
        Gizmos.color = Color.red;
        Vector3 initialDir = startRot * Vector3.right;
        Gizmos.DrawLine(
            startPos - initialDir * rotationLineLength,
            startPos + initialDir * rotationLineLength
        );

        // --- 3. Final rotation (Green) ---
        Gizmos.color = Color.green;
        Vector3 finalDir = destinedRotation * Vector3.right;
        Gizmos.DrawLine(
            destinedPosition - finalDir * rotationLineLength,
            destinedPosition + finalDir * rotationLineLength
        );
    }
    public void SetColour(Color colour)
    {
        if (colourSprite != null)
        {
            colourSprite.color = colour;
        }
    }
    private void PlayMovingSound()
    {
        movingSoundAudioSource.pitch = Random.Range(0.8f, 1.2f);
        movingSoundAudioSource.Play();
    }
    private void PlayMovingStopSound()
    {
        movingStopSoundAudioSource.pitch = movingSoundAudioSource.pitch;
        movingStopSoundAudioSource.Play();
    }
}
