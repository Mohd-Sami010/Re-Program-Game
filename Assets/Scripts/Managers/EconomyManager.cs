using UnityEngine;

public class EconomyManager :MonoBehaviour {
    public static EconomyManager Instance { get; private set; }
    [SerializeField] private string currencyName = "grears";
    [SerializeField] private int currentBalance;
    [SerializeField] private bool setInspectorBalance;

    [Header("Prices")]
    [SerializeField] private int healthPrice_70percent = 27;
    [SerializeField] private int energyPrice_100percent = 15;

    [Space]
    [Header("Level Reward")]
    [SerializeField] private int levelCompleteReward = 0;

    public event System.Action OnTransactionSucess;
    public event System.Action OnTransactionFail;
    private void Awake()
    {
        Instance = this;
        if (setInspectorBalance) PlayerPrefs.SetInt("balance", currentBalance);
        else currentBalance = PlayerPrefs.GetInt("balance", 0);

        levelCompleteReward = 12 + (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex * 1);
    }
    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver(object sender, GameManager.OnGameOverEventArgs e)
    {
        if (e.gameOverType == GameManager.GameOverType.win)
        {
            AddCurrency(levelCompleteReward);
        }
    }

    public int GetCurrentBalance()
    {
        return currentBalance;
    }
    public string GetCurrencyName()
    {
        return currencyName;
    }

    public bool TrySpendCurrency(int amount)
    {
        if (currentBalance >= amount)
        {
            currentBalance -= amount;
            SaveBalance();
            OnTransactionSucess?.Invoke();
            return true;
        }
        OnTransactionFail?.Invoke();
        return false;
    }
    public void AddCurrency(int amount)
    {
        currentBalance += amount;
        SaveBalance();
    }
    private void SaveBalance()
    {
        PlayerPrefs.SetInt("balance", currentBalance);
    }

    #region Prices Getters
    public int GetHealthPrice_70percent()
    {
        return healthPrice_70percent;
    }
    public int GetEnergyPrice_100percent()
    {
        return energyPrice_100percent;
    }
    #endregion

    public int GetLevelCompleteReward()
    {
        return levelCompleteReward;
    }
}
