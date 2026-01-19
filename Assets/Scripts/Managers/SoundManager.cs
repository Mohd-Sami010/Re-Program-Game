using UnityEngine;

public class SoundManager :MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    [Header("Robot Sounds")]
    [SerializeField] private AudioSource robotMoveAudioSource;
    [SerializeField] private AudioSource robotJumpAudioSource;
    [SerializeField] private AudioSource robotTurnAudioSource;
    [SerializeField] private AudioSource robotLoseAudioSource;
    [SerializeField] private AudioSource robotWinAudioSource;
    [SerializeField] private AudioSource robotHurtAudioSource;

    [Header("HUD Sounds")]
    [SerializeField] private AudioSource playButtonAudioSource;
    [SerializeField] private AudioSource stopButtonAudioSource;

    [Header("Snippet Sounds")]
    [SerializeField] private AudioSource snippetGrabAudioSource;
    [SerializeField] private AudioSource snippetDropAudioSource;
    [SerializeField] private AudioSource snippetSpawnAudioSource;
    [SerializeField] private AudioSource snippetDeleteAudioSource;

    [Header("UI Sounds")]
    [SerializeField] private AudioSource uISound1AudioSource;
    [SerializeField] private AudioSource uISound2AudioSource;

    [Header("Money Sound")]
    [SerializeField] private AudioSource transactionSucessAudioSource;
    [SerializeField] private AudioSource transactionFailAudioSource;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        EconomyManager economyManager = EconomyManager.Instance;
        if (economyManager == null) return;
        EconomyManager.Instance.OnTransactionSucess += EconomyManager_OnTransactionSucess;
        EconomyManager.Instance.OnTransactionFail += EconomyManager_OnTransactionFail;

        GameManager.Instance.OnGamePause += GameManager_OnGamePaused;
        GameManager.Instance.OnGameResume += GameManager_OnGameResumed;
    }
    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        if (robotMoveAudioSource.isPlaying) robotMoveAudioSource.Pause();
        if (robotJumpAudioSource.isPlaying) robotJumpAudioSource.Pause();
    }
    private void GameManager_OnGameResumed(object sender, System.EventArgs e)
    {
        if (robotMoveAudioSource.clip != null) robotMoveAudioSource.UnPause();
        if (robotJumpAudioSource.clip != null) robotJumpAudioSource.UnPause();
    }

    private void EconomyManager_OnTransactionSucess()
    {
        PlaySound(transactionSucessAudioSource);
    }
    private void EconomyManager_OnTransactionFail()
    {
        PlaySound(transactionFailAudioSource);
    }
    // Robot Sounds
    public void PlayRobotMoveSound()
    {
        PlaySound(robotMoveAudioSource);
    }
    public void StopRobotMoveSound()
    {
        if (robotMoveAudioSource == null) return;
        robotMoveAudioSource.Stop();
    }
    public void PlayRobotJumpSound()
    {
        PlaySound(robotJumpAudioSource);
    }
    public void StopRobotJumpSound()
    {
        if (robotJumpAudioSource == null) return;
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
    public void PlayRobotHurtSound()
    {
        PlaySound(robotHurtAudioSource);
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
        if (audioSource == null) return;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioSource.clip);
    }
    private void OnDestroy()
    {
        if (EconomyManager.Instance == null) return;
        EconomyManager.Instance.OnTransactionSucess -= EconomyManager_OnTransactionSucess;
        EconomyManager.Instance.OnTransactionFail -= EconomyManager_OnTransactionFail;

        if (GameManager.Instance == null) return;
        GameManager.Instance.OnGamePause -= GameManager_OnGamePaused;
        GameManager.Instance.OnGameResume -= GameManager_OnGameResumed;
    }
}
