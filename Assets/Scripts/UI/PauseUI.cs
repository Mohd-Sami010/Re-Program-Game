using UnityEngine;
using UnityEngine.UI;

public class PauseUI :MonoBehaviour {

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.TogglePauseGame();
            SoundManager.Instance.PlayUISound1();
            gameObject.SetActive(false);
        });
        menuButton.onClick.AddListener(() => {
            GameManager.Instance.LoadMainMenu();
            SoundManager.Instance.PlayUISound1();
        });
        GameManager.Instance.OnGamePause += GameManager_OnGamePause;
        gameObject.SetActive(false);
    }
    private void GameManager_OnGamePause(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
