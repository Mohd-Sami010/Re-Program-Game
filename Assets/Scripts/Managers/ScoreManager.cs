using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager :MonoBehaviour {
    public static ScoreManager Instance { get; private set; }

    [Header("Balancing")]
    [SerializeField] private int baseReward = 5;
    [SerializeField] private int maxTimeBonus = 10;
    [SerializeField] private int maxSnippetBonus = 8;

    [SerializeField] private int minExpectedLevelPlayTime = 30;
    [SerializeField] private int minNumOfSnippets = 5;

    // TMP tab stop position (tweak once, done forever)
    private const int RIGHT_COLUMN = 460;

    private void Awake()
    {
        Instance = this;
    }

    // -------------------- CALCULATION --------------------

    private int CalculateTimeBonus()
    {
        int actualTime = GameManager.Instance.GetLevelPlayTime();
        if (actualTime >= minExpectedLevelPlayTime) return 0;

        float efficiency = 1f - (float)actualTime / minExpectedLevelPlayTime;
        return Mathf.RoundToInt(efficiency * maxTimeBonus);
    }

    private int CalculateSnippetBonus()
    {
        int actualSnippets = SnippetsManagerUI.Instance.GetNumberOfSnippetsUsed();
        if (actualSnippets >= minNumOfSnippets) return 0;

        float efficiency = 1f - (float)actualSnippets / minNumOfSnippets;
        return Mathf.RoundToInt(efficiency * maxSnippetBonus);
    }

    public int GetLevelReward()
    {
        return baseReward + CalculateTimeBonus() + CalculateSnippetBonus();
    }

    // -------------------- REPORT --------------------

    public string GetLevelReport()
    {
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        string currency = EconomyManager.Instance.GetCurrencyName();

        int timeBonus = CalculateTimeBonus();
        int snippetBonus = CalculateSnippetBonus();
        int total = GetLevelReward();

        string tab = $"<pos={RIGHT_COLUMN}>";

        return
            $"<b><size=105%>LEVEL {levelNumber} RESULT</size></b>\n\n" +

            $"Base reward{tab}  {baseReward} {currency}\n" +
            $"Time bonus{tab}{FormatBonus(timeBonus, currency)}\n" +
            $"Code efficiency{tab}{FormatBonus(snippetBonus, currency)}\n\n" +

            $"<color=#FFD200><b>TOTAL EARNED{tab}{total} {currency}</b></color>";
    }

    private string FormatBonus(int value, string currency)
    {
        return value > 0
            ? $"<color=#00FF6A>+{value} {currency}</color>"
            : $"<color=#666666>+0 {currency}</color>";
    }
}
