using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI :MonoBehaviour {

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextLevelButton;

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameOver_OnGameOver;

        retryButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.StopGame();
            gameObject.SetActive(false);
            
        });
        nextLevelButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.LoadNextLevel();
            
        });
        gameObject.SetActive(false);
    }

    private void GameOver_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        gameObject.SetActive(true);
        if (e.gameOverType == GameManager.GameOverType.win)
        {
            titleText.text = "Level Completed!";
            retryButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            titleText.text = "Game Over";
            retryButton.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(false);
        }
    }
}
