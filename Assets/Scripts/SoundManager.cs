using UnityEngine;

public class SoundManager :MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    [Header("Robot Sounds")]
    [SerializeField] private AudioSource robotMoveAudioSource;
    [SerializeField] private AudioSource robotJumpAudioSource;
    [SerializeField] private AudioSource robotTurnAudioSource;
    [SerializeField] private AudioSource robotLoseAudioSource;
    [SerializeField] private AudioSource robotWinAudioSource;

    [Header("HUD Sounds")]
    [SerializeField] private AudioSource playButtonAudioSource;
    [SerializeField] private AudioSource stopButtonAudioSource;

    [Header("Snippet Sounds")]
    [SerializeField] private AudioSource snippetGrabAudioSource;
    [SerializeField] private AudioSource snippetDropAudioSource;
    [SerializeField] private AudioSource snippetSpawnAudioSource;
    [SerializeField] private AudioSource snippetDeleteAudioSource;

    [Header("Other Sounds")]
    [SerializeField] private AudioSource uISound1AudioSource;
    [SerializeField] private AudioSource uISound2AudioSource;

    private void Awake()
    {
        Instance = this;
    }

    // Robot Sounds
    public void PlayRobotMoveSound()
    {
        PlaySound(robotMoveAudioSource);
    }
    public void StopRobotMoveSound()
    {
        robotMoveAudioSource.Stop();
    }
    public void PlayRobotJumpSound()
    {
        PlaySound(robotJumpAudioSource);
    }
    public void StopRobotJumpSound()
    {
        robotJumpAudioSource.Stop();
    }
    public void PlayRobotTurnSound()
    {
        PlaySound(robotTurnAudioSource);
    }
    public void PlayRobotLoseSound()
    {
        PlaySound(robotLoseAudioSource);
    }
    public void PlayRobotWinSound()
    {
        PlaySound(robotWinAudioSource);
    }

    // HUD Sounds
    public void PlayPlayButtonSound()
    {
        PlaySound(playButtonAudioSource);
    }
    public void PlayStopButtonSound()
    {
        PlaySound(stopButtonAudioSource);
    }

    // Snippet Sounds
    public void PlaySnippetGrabSound()
    {
        PlaySound(snippetGrabAudioSource);
    }
    public void PlaySnippetDropSound()
    {
        PlaySound(snippetDropAudioSource);
    }
    public void PlaySnippetSpawnSound()
    {
        PlaySound(snippetSpawnAudioSource);
    }
    public void PlaySnippetDeleteSound()
    {
        PlaySound(snippetDeleteAudioSource);
    }

    // Other Sounds
    public void PlayUISound1()
    {
        PlaySound(uISound1AudioSource);
    }
    public void PlayUISound2()
    {
        PlaySound(uISound2AudioSource);
    }

    private void PlaySound(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
