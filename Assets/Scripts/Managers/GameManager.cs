using System;
using UnityEngine;

public class GameManager :MonoBehaviour {
    public static GameManager Instance { get; private set; }

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
    public event EventHandler OnContinueGame;
    private void Awake()
    {
        Instance = this;
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
    //public void ContinueGame()
    //{
    //    OnContinueGame?.Invoke(this, EventArgs.Empty);
    //    StopGame();
    //}
    public void StopGame()
    {
        currentGameState= GameState.NotRunning;
        OnGameStop?.Invoke(this, EventArgs.Empty);
    }
    public void GameOver(GameOverType gameOverType)
    {
        currentGameState = GameState.GameOver;
        OnGameOver?.Invoke(this, new OnGameOverEventArgs { gameOverType = gameOverType });
        if (gameOverType == GameOverType.win) SoundManager.Instance.PlayRobotWinSound();
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
}
