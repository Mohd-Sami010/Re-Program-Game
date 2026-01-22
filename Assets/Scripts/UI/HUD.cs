using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD :MonoBehaviour {

    public static HUD Instance { get; private set; }

    [SerializeField] private Button playButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button editSnippetsButton;

    [SerializeField] private GameObject snippetsUI;

    [Header("Health and Energy")]
    [SerializeField] private Image healthBarImage;
    [SerializeField] private GameObject lowHealthWarningImage;
    [SerializeField] private Image energyBarImage;
    [SerializeField] private GameObject lowEnergyWarningImage;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (SnippetsManagerUI.Instance != null)
        {
            SnippetsManagerUI.Instance.OnSnippetsListChange += SnippetsManagerUI_OnSnippetsListChange;
        }
        playButton.onClick.AddListener(() => {
            GameManager.Instance.RestartGame();
            DisableRunButton();
            SoundManager.Instance.PlayPlayButtonSound();

        });
        restartButton.onClick.AddListener(() => {
            GameManager.Instance.RestartGame();
            SoundManager.Instance.PlayPlayButtonSound();
        });
        stopButton.onClick.AddListener(() => {
            GameManager.Instance.StopGame();
            SoundManager.Instance.PlayStopButtonSound();

            EnableRunButton();
        });
        pauseButton.onClick.AddListener(() => {
            GameManager.Instance.TogglePauseGame();
            SoundManager.Instance.PlayUISound1();
        });
        editSnippetsButton.onClick.AddListener(() => {
            snippetsUI.SetActive(true);
            HideEditButton();
            SoundManager.Instance.PlayUISound1();
        });

        EnableRunButton();

        GameManager.Instance.OnGameStop += GameManager_OnGameStop;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        GameManager.Instance.OnGamePause += GameManager_OnGamePause;
        GameManager.Instance.OnGameResume += GameManager_OnGameResume;
        GameManager.Instance.OnRevived += GameManager_OnRevived;
        RobotHealthAndEnergy.Instance.OnHealthOrEnergyChanged += RobotHealthAndEnergy_OnHealthOrEnergyChanged;
        RobotHealthAndEnergy.Instance.OnRobotDamage += RobotHealthAndEnergy_OnRobotDamage;
    }

    private void SnippetsManagerUI_OnSnippetsListChange()
    {
        if (SnippetsManagerUI.Instance.GetNumberOfSnippetsUsed() > 0)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    private void GameManager_OnRevived()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Enable");
        EnableRunButton();
        ShowEditButton();
    }

    private void GameManager_OnGamePause(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }
    private void GameManager_OnGameResume(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Enable");
        ShowEditButton();
    }
    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        StartCoroutine(DisableUI());
    }

    private void RobotHealthAndEnergy_OnHealthOrEnergyChanged(object sender, RobotHealthAndEnergy.OnHealthOrEnergyChangedEventArgs e)
    {
        healthBarImage.fillAmount = e.robotHealth / 100;
        energyBarImage.fillAmount = e.robotEnergy / 100;

        if (e.robotHealth <= 7) lowHealthWarningImage.SetActive(true);
        else lowHealthWarningImage.SetActive(false);

        if (e.robotEnergy <= 7) lowEnergyWarningImage.SetActive(true);
        else lowEnergyWarningImage.SetActive(false);
    }
    private void RobotHealthAndEnergy_OnRobotDamage()
    {
        healthBarImage.transform.parent.parent.GetComponent<Animator>().SetTrigger("Shake");
    }

    private void GameManager_OnGameStop(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        EnableRunButton();
        ShowEditButton();
    }

    public void ShowEditButton()
    {
        editSnippetsButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
    }
    private void HideEditButton()
    {
        editSnippetsButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStop -= GameManager_OnGameStop;
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
        GameManager.Instance.OnGamePause -= GameManager_OnGamePause;
        GameManager.Instance.OnGameResume -= GameManager_OnGameResume;
        RobotHealthAndEnergy.Instance.OnHealthOrEnergyChanged -= RobotHealthAndEnergy_OnHealthOrEnergyChanged;
    }
    private void EnableRunButton()
    {
        playButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(false);
    }
    private void DisableRunButton()
    {
        playButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(true);
    }

    private IEnumerator DisableUI()
    {
        GetComponent<Animator>().SetTrigger("Disable");
        yield return new WaitForSecondsRealtime(0.3f);
        gameObject.SetActive(false);
    }
}
