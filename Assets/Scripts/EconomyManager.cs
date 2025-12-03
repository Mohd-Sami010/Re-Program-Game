using UnityEngine;

public class EconomyManager :MonoBehaviour {
    public static EconomyManager Instance { get; private set; }
    [SerializeField] private string currencyName = "grears";
    [SerializeField] private int currentBalance;

    [Header("Prices")]
    [SerializeField] private int healthPrice_70percent = 27;
    [SerializeField] private int energyPrice_100percent = 15;

    private void Awake()
    {
        Instance = this;
        currentBalance = PlayerPrefs.GetInt("balance", 0);
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
            return true;
        }
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
}
