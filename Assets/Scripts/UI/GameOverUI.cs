using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI :MonoBehaviour {

    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Next Level")]
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TextMeshProUGUI rewardTextMesh;
    [SerializeField] private TextMeshProUGUI currentBalanceTextMesh;

    [Header("Buy Health")]
    [SerializeField] private GameObject buyHealthUi;
    [SerializeField] private Button adForHealthButton;
    [SerializeField] private Button buyHealthButton;
    [SerializeField] private TextMeshProUGUI buyHealthTextMesh;

    [Header("Buy Energy")]
    [SerializeField] private GameObject buyEnergyUi;
    [SerializeField] private Button adForEnergyButton;
    [SerializeField] private Button buyEnergyButton;
    [SerializeField] private TextMeshProUGUI buyEnergyTextMesh;

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
            if (EconomyManager.Instance.TrySpendCurrency(EconomyManager.Instance.GetHealthPrice_70percent()))
            {
                RobotHealthAndEnergy.Instance.AddHealth(70);
                GameManager.Instance.StopGame();
                gameObject.SetActive(false);
            }
            else
            {
                // Not enough currency
                //SoundManager.Instance.PlayErrorSound();
            }
        });
        buyHealthTextMesh.text = $"70% health ({EconomyManager.Instance.GetHealthPrice_70percent()} {EconomyManager.Instance.GetCurrencyName()})";

        // Energy
        adForEnergyButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.StopGame();
            RobotHealthAndEnergy.Instance.AddEnergy(60);
            gameObject.SetActive(false);
        });
        buyEnergyButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();

            if (EconomyManager.Instance.TrySpendCurrency(EconomyManager.Instance.GetEnergyPrice_100percent()))
            {
                RobotHealthAndEnergy.Instance.AddEnergy(100);
                GameManager.Instance.StopGame();
                gameObject.SetActive(false);
            }
            else
            {
                // Not enough currency
                //SoundManager.Instance.PlayErrorSound();
            }
        });
        buyEnergyTextMesh.text = $"100% energy ({EconomyManager.Instance.GetEnergyPrice_100percent()} {EconomyManager.Instance.GetCurrencyName()})";

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
            rewardTextMesh.text = $"{EconomyManager.Instance.GetLevelCompleteReward()} {EconomyManager.Instance.GetCurrencyName()}";
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
        
        currentBalanceTextMesh.text = $"{EconomyManager.Instance.GetCurrentBalance()} {EconomyManager.Instance.GetCurrencyName()}";
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= GameOver_OnGameOver;
    }
}
