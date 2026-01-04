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

        int timeBonus = CalculateTimeBonus();
        int snippetBonus = CalculateSnippetBonus();
        int total = GetLevelReward();

        string tab = $"<pos={RIGHT_COLUMN}>";

        return
            $"\nBase reward{tab}  {baseReward}\n" +
            $"Time bonus{tab}  {FormatBonus(timeBonus)}\n" +
            $"Code efficiency{tab}  {FormatBonus(snippetBonus)}\n\n" +

            $"<color=#D4D4D4><b>TOTAL{tab}  {EconomyManager.Instance.GetCurrentBalance()}</b></color>";
    }

    private string FormatBonus(int value)
    {
        return value > 0
            ? $"<color=#FFFFFF>{value}</color>"
            : $"<color=#666666>0</color>";
    }
}
