using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI :MonoBehaviour {

    private int levelReward;

    [Header("Next Level")]
    [SerializeField] private GameObject nextLevelUi;
    [SerializeField] private TextMeshProUGUI winTitleText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TextMeshProUGUI levelReportTextMesh;
    [SerializeField] private CanvasGroup currentBalanceCanvasGroup;
    [SerializeField] private TextMeshProUGUI currentBalanceTextMesh;

    [Space]
    [Header("Lose")]
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
            GameManager.Instance.LoadNextLevel();
        });

        #region Buy Health Buttons
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
                GameManager.Instance.ReviveAndContinue();
                StartCoroutine(PlayUiDisableAnim());
            });
        });

        buyHealthButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            if (EconomyManager.Instance.TrySpendCurrency(EconomyManager.Instance.GetHealthPrice_70percent()))
            {
                RobotHealthAndEnergy.Instance.AddHealth(70);
                GameManager.Instance.ReviveAndContinue();
                StartCoroutine(PlayUiDisableAnim());
            }
            else
            {
                // Not enough currency
                //SoundManager.Instance.PlayErrorSound();
            }
        });
        buyHealthTextMesh.text = $"70% Health      {EconomyManager.Instance.GetHealthPrice_70percent()}";
        #endregion
        #region Buy Energy Buttons
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
                GameManager.Instance.ReviveAndContinue();
                StartCoroutine(PlayUiDisableAnim());
            });
        });


        buyEnergyButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();

            if (EconomyManager.Instance.TrySpendCurrency(EconomyManager.Instance.GetEnergyPrice_100percent()))
            {
                RobotHealthAndEnergy.Instance.AddEnergy(100);
                GameManager.Instance.ReviveAndContinue();
                StartCoroutine(PlayUiDisableAnim());
            }
            else
            {
                // Not enough currency
                //SoundManager.Instance.PlayErrorSound();
            }
        });
        buyEnergyTextMesh.text = $"100% Battery      {EconomyManager.Instance.GetEnergyPrice_100percent()}";
        #endregion

        ResetUI();
        adLoadingUI.SetActive(false);
        gameObject.SetActive(false);
    }

    private void GameOver_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {

        ResetUI();
        currentBalanceTextMesh.text = EconomyManager.Instance.GetCurrentBalance().ToString();

        gameObject.SetActive(true);

        if (e.gameOverType == GameManager.GameOverType.win) GetComponent<Animator>().SetTrigger("EnableWin");
        else GetComponent<Animator>().SetTrigger("EnableLose");

        if (e.gameOverType == GameManager.GameOverType.win)
        {
            nextLevelUi.SetActive(true);
            levelReward = ScoreManager.Instance.GetLevelReward();
            EconomyManager.Instance.AddCurrency(levelReward);

            winTitleText.gameObject.SetActive(true);
            winTitleText.text = "Level Completed!";

            levelReportTextMesh.text = ScoreManager.Instance.GetLevelReport();
        }
        else if (e.gameOverType == GameManager.GameOverType.robotDied)
        {
            currentBalanceTextMesh.transform.parent.parent.gameObject.SetActive(true);
            loseTitleText.gameObject.SetActive(true);
            loseTitleText.text = "Robot Died";
            buyHealthUi.SetActive(true);
            if (EconomyManager.Instance.GetHealthPrice_70percent() > EconomyManager.Instance.GetCurrentBalance())
            {
                buyHealthTextMesh.text = "<color=#FF6666>70% Health      " + EconomyManager.Instance.GetHealthPrice_70percent() + "</color>";
            }
        }
        else if (e.gameOverType == GameManager.GameOverType.robotOutOfEnergy)
        {
            currentBalanceTextMesh.transform.parent.parent.gameObject.SetActive(true);
            loseTitleText.gameObject.SetActive(true);
            loseTitleText.text = "No Battery";
            buyEnergyUi.SetActive(true);

            if (EconomyManager.Instance.GetEnergyPrice_100percent() > EconomyManager.Instance.GetCurrentBalance())
            {
                buyEnergyTextMesh.text = "<color=#FF6666>100% Battery      " + EconomyManager.Instance.GetEnergyPrice_100percent() + "</color>";
                //buyEnergyButton.interactable = false;
            }
        }
        
        currentBalanceTextMesh.text = $"{EconomyManager.Instance.GetCurrentBalance()} {EconomyManager.Instance.GetCurrencyName()}";

    }
    private IEnumerator PlayUiDisableAnim()
    {
        GetComponent<Animator>().SetTrigger("Disable");
        yield return new WaitForSecondsRealtime(0.3f);
        gameObject.SetActive(false);
    }
    private void ResetUI()
    {
        nextLevelUi.SetActive(false);
        buyHealthUi.SetActive(false);
        buyEnergyUi.SetActive(false);
        winTitleText.gameObject.SetActive(false);
        loseTitleText.gameObject.SetActive(false);
        currentBalanceTextMesh.transform.parent.parent.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= GameOver_OnGameOver;
    }
}
