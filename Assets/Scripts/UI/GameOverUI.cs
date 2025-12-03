using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI :MonoBehaviour {

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button nextLevelButton;

    [Header("Buy Health")]
    [SerializeField] private GameObject buyHealthUi;
    [SerializeField] private Button adForHealthButton;
    [SerializeField] private Button buyHealthButton;

    [Header("Buy Energy")]
    [SerializeField] private GameObject buyEnergyUi;
    [SerializeField] private Button adForEnergyButton;
    [SerializeField] private Button buyEnergyButton;

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameOver_OnGameOver;

        nextLevelButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.LoadNextLevel();
            
        });
        // Health
        adForHealthButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.StopGame();
            RobotHealthAndEnergy.Instance.AddHealth(40);
            gameObject.SetActive(false);
        });
        buyHealthButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.StopGame();
            if (EconomyManager.Instance.TrySpendCurrency(EconomyManager.Instance.GetHealthPrice_70percent()))
            {
                RobotHealthAndEnergy.Instance.AddHealth(100);
                gameObject.SetActive(false);
            }
            else
            {
                // Not enough currency
                //SoundManager.Instance.PlayErrorSound();
            }
            RobotHealthAndEnergy.Instance.AddHealth(70);
            gameObject.SetActive(false);
        });

        // Energy
        adForEnergyButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.StopGame();
            RobotHealthAndEnergy.Instance.AddEnergy(60);
            gameObject.SetActive(false);
        });
        buyEnergyButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.StopGame();

            if (EconomyManager.Instance.TrySpendCurrency(EconomyManager.Instance.GetEnergyPrice_100percent()))
            {
                RobotHealthAndEnergy.Instance.AddEnergy(100);
                gameObject.SetActive(false);
            }
            else
            {
                // Not enough currency
                //SoundManager.Instance.PlayErrorSound();
            }
        });

        gameObject.SetActive(false);
        buyHealthUi.SetActive(false);
        buyEnergyUi.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
    }

    private void GameOver_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        gameObject.SetActive(true);
        buyHealthUi.SetActive(false);
        buyEnergyUi.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);

        if (e.gameOverType == GameManager.GameOverType.win)
        {
            titleText.text = "Level Completed!";
            nextLevelButton.gameObject.SetActive(true);
        }
        else if (e.gameOverType == GameManager.GameOverType.robotDied)
        {
            titleText.text = "Robot Died";
            buyHealthUi.SetActive(true);
        }
        else if (e.gameOverType == GameManager.GameOverType.robotOutOfEnergy)
        {
            titleText.text = "No Energy";
            buyEnergyUi.SetActive(true);
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= GameOver_OnGameOver;
    }
}
