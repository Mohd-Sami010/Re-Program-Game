using System;
using UnityEngine;

public class GameManager :MonoBehaviour {
    public static GameManager Instance { get; private set; }

    private float levelPlayTime = 0;
    [SerializeField] private string levelTimeStr;
    public enum GameState
    {
        NotRunning,
        Running,
        GameOver
    }
    private GameState currentGameState = GameState.NotRunning;
    public enum GameOverType{
        win,
        robotDied,
        robotOutOfEnergy
    }
    public event EventHandler OnGameRestart;
    public event EventHandler OnGameStop;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameResume;
    public event EventHandler<OnGameOverEventArgs> OnGameOver;
    public class OnGameOverEventArgs : EventArgs {
        public GameOverType gameOverType;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        levelTimeStr = GetLevelPlayTimeString();
        if (currentGameState == GameState.NotRunning || currentGameState == GameState.Running)
        {
            levelPlayTime += Time.deltaTime;
        }
    }
    public void RestartGame()
    {
        currentGameState = GameState.Running;
        OnGameRestart?.Invoke(this, EventArgs.Empty);
    }
    public void TogglePauseGame()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            currentGameState = GameState.NotRunning;
            OnGamePause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            currentGameState = GameState.Running;
            OnGameResume?.Invoke(this, EventArgs.Empty);
        }
    }
    public void StopGame()
    {
        currentGameState= GameState.NotRunning;
        OnGameStop?.Invoke(this, EventArgs.Empty);
    }
    public void GameOver(GameOverType gameOverType)
    {
        currentGameState = GameState.GameOver;
        OnGameOver?.Invoke(this, new OnGameOverEventArgs { gameOverType = gameOverType });
        if (gameOverType == GameOverType.win)
        {
            PlayerPrefs.SetInt("Level_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + "_Completed", 1);
            PlayerPrefs.SetInt("LevelToContinue", UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
            SoundManager.Instance.PlayRobotWinSound();
        }
        else SoundManager.Instance.PlayRobotLoseSound();
    }
    public void LoadNextLevel()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // No more levels, restart the first level or show a message
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    private void OnDestroy()
    {
        currentGameState = GameState.NotRunning;
    }
    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }
    public String GetLevelPlayTimeString()
    {
        string timeString = "";
        int levelTime = (int)Mathf.Floor(levelPlayTime);
        if (levelTime >= 60) timeString += (int)(levelTime / 60) + "mins ";
        timeString += levelTime % 60 + "s";
        return timeString;

    }
    public int GetLevelPlayTime()
    {
        return (int)levelPlayTime;
    }
}
