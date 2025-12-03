using System;
using UnityEngine;

public class GameManager :MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public bool isGameplayRunning { get; private set; }

    public enum GameOverType{
        win,
        robotDied,
        robotOutOfEnergy
    }
    public event EventHandler OnGameRestart;
    public event EventHandler OnGameStop;
    public event EventHandler<OnGameOverEventArgs> OnGameOver;
    public class OnGameOverEventArgs : EventArgs {
        public GameOverType gameOverType;
    }
    private void Awake()
    {
        Instance = this;
    }
    public void RestartGame()
    {
        OnGameRestart?.Invoke(this, EventArgs.Empty);
        isGameplayRunning = true;
    }
    public void StopGame()
    {
        OnGameStop?.Invoke(this, EventArgs.Empty);
        isGameplayRunning = false;
    }
    public void GameOver(GameOverType gameOverType)
    {
        OnGameOver?.Invoke(this, new OnGameOverEventArgs { gameOverType = gameOverType });
        if (gameOverType == GameOverType.win) SoundManager.Instance.PlayRobotWinSound();
        else if (gameOverType == GameOverType.robotDied) SoundManager.Instance.PlayRobotLoseSound();
        isGameplayRunning = false;
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
}
