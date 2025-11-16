using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD :MonoBehaviour {

    public static HUD Instance { get; private set; }

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI playButtonText;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button editSnippetsButton;

    [SerializeField] private GameObject snippetsUI;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        playButton.onClick.AddListener(() => {
            GameManager.Instance.RestartGame();
            playButtonText.text = "Restart";
            SoundManager.Instance.PlayPlayButtonSound();
            stopButton.gameObject.SetActive(true);
        });
        stopButton.onClick.AddListener(() => {
            GameManager.Instance.StopGame();
            SoundManager.Instance.PlayStopButtonSound();
        });
        editSnippetsButton.onClick.AddListener(() => {
            snippetsUI.SetActive(true);
            editSnippetsButton.gameObject.SetActive(false);
            SoundManager.Instance.PlayUISound1();
        });

        stopButton.gameObject.SetActive(false);
        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
    }

    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        playButtonText.text = "Run";
        stopButton.gameObject.SetActive(false);
    }

    public void ShowEditButton()
    {
        editSnippetsButton.gameObject.SetActive(true);
    }

}
