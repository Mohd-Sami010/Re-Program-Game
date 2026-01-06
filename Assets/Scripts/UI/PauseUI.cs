using UnityEngine;
using UnityEngine.UI;

public class PauseUI :MonoBehaviour {

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsUi;

    private void Start()
    {
        resumeButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.TogglePauseGame();
            gameObject.SetActive(false);
        });
        menuButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.LoadMainMenu();
        });
        settingsButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            settingsUi.SetActive(true);
        });
        GameManager.Instance.OnGamePause += GameManager_OnGamePause;
        gameObject.SetActive(false);

    }
    private void GameManager_OnGamePause(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
