using UnityEngine;

public class EconomyManager :MonoBehaviour {

    [SerializeField] private string currencyName = "grears";
    [SerializeField] private int currentBalance;

    private void Awake()
    {
        currentBalance = PlayerPrefs.GetInt("balance", 0);
    }

}
