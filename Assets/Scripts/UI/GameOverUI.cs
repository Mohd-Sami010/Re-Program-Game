using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI :MonoBehaviour {


    [SerializeField] private float balanceFadeAlpha = 0.25f;
    [SerializeField] private float balanceAnimDuration = 0.5f;

    private int cachedBalanceBeforeReward;
    private int levelReward;


    [Header("Next Level")]
    [SerializeField] private TextMeshProUGUI winTitleText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TextMeshProUGUI levelReportTextMesh;
    [SerializeField] private TextMeshProUGUI currentBalanceTextMesh;


    [SerializeField] private TextMeshProUGUI loseTitleText;
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

    [Header("Ads UI")]
    [SerializeField] private GameObject adLoadingUI;


    private void Start()
    {
        GameManager.Instance.OnGameOver += GameOver_OnGameOver;

        nextLevelButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayUISound1();
            StartCoroutine(ApplyRewardAndLoadNextLevel());
        });
        // Health
        adForHealthButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayUISound1();

            adLoadingUI.SetActive(true);

            AdManager.Instance.ShowRewardedAdWithWait(5f, watched =>
            {
                adLoadingUI.SetActive(false);

                if (!watched)
                {
                    // Optional: show "Ad not available"
                    return;
                }

                RobotHealthAndEnergy.Instance.AddHealth(40);
                GameManager.Instance.StopGame();
                gameObject.SetActive(false);
            });
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
        adForEnergyButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayUISound1();

            adLoadingUI.SetActive(true);

            AdManager.Instance.ShowRewardedAdWithWait(5f, watched =>
            {
                adLoadingUI.SetActive(false);

                if (!watched)
                {
                    return;
                }

                RobotHealthAndEnergy.Instance.AddEnergy(60);
                GameManager.Instance.StopGame();
                gameObject.SetActive(false);
            });
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
        winTitleText.gameObject.SetActive(false);
        loseTitleText.gameObject.SetActive(false);
        levelReportTextMesh.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
    }

    private void GameOver_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        gameObject.SetActive(true);
        buyHealthUi.SetActive(false);
        buyEnergyUi.SetActive(false);
        winTitleText.gameObject.SetActive(false);
        loseTitleText.gameObject.SetActive(false);
        levelReportTextMesh.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);

        currentBalanceTextMesh.transform.parent.parent.gameObject.SetActive(false);

        if (e.gameOverType == GameManager.GameOverType.win)
        {
            winTitleText.gameObject.SetActive(true);
            winTitleText.text = "Level Completed!";
            nextLevelButton.gameObject.SetActive(true);

            levelReward = ScoreManager.Instance.GetLevelReward();
            levelReportTextMesh.gameObject.SetActive(true);
            levelReportTextMesh.text = ScoreManager.Instance.GetLevelReport();

            // Cache balance BEFORE reward
            cachedBalanceBeforeReward = EconomyManager.Instance.GetCurrentBalance();

            // Dim / hide balance during result screen
            currentBalanceTextMesh.text =
                $"{cachedBalanceBeforeReward} {EconomyManager.Instance.GetCurrencyName()}";

            currentBalanceTextMesh.alpha = balanceFadeAlpha;
            currentBalanceTextMesh.gameObject.SetActive(false);
        }
        else if (e.gameOverType == GameManager.GameOverType.robotDied)
        {
            currentBalanceTextMesh.transform.parent.parent.gameObject.SetActive(true);
            loseTitleText.gameObject.SetActive(true);
            loseTitleText.text = "Robot Died";
            buyHealthUi.SetActive(true);
        }
        else if (e.gameOverType == GameManager.GameOverType.robotOutOfEnergy)
        {
            currentBalanceTextMesh.transform.parent.parent.gameObject.SetActive(true);
            loseTitleText.gameObject.SetActive(true);
            loseTitleText.text = "No Energy";
            buyEnergyUi.SetActive(true);
        }
        else
        {
            currentBalanceTextMesh.gameObject.SetActive(true);
            currentBalanceTextMesh.alpha = 1f;
            currentBalanceTextMesh.text =
                $"{EconomyManager.Instance.GetCurrentBalance()} {EconomyManager.Instance.GetCurrencyName()}";
        }
        
        currentBalanceTextMesh.text = $"{EconomyManager.Instance.GetCurrentBalance()} {EconomyManager.Instance.GetCurrencyName()}";
    }
    private System.Collections.IEnumerator ApplyRewardAndLoadNextLevel()
    {

        // Apply reward NOW (player already saw breakdown)
        EconomyManager.Instance.AddCurrency(levelReward);

        int startValue = cachedBalanceBeforeReward;
        int endValue = EconomyManager.Instance.GetCurrentBalance();

        currentBalanceTextMesh.gameObject.SetActive(true);
        currentBalanceTextMesh.transform.parent.parent.gameObject.SetActive(true);

        float t = 0f;
        while (t < balanceAnimDuration)
        {
            t += Time.unscaledDeltaTime;
            float lerp = Mathf.Clamp01(t / balanceAnimDuration);
            int value = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, lerp));
            currentBalanceTextMesh.text = $"{value} {EconomyManager.Instance.GetCurrencyName()}";
            yield return null;
        }

        currentBalanceTextMesh.text = $"{endValue} {EconomyManager.Instance.GetCurrencyName()}";

        yield return new WaitForSecondsRealtime(0.15f);

        GameManager.Instance.LoadNextLevel();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= GameOver_OnGameOver;
    }
}
