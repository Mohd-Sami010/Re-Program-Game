using Unity.VisualScripting;
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

    private bool isLevelCompleted;

    private void Awake()
    {
        Instance = this;

        isLevelCompleted = PlayerPrefs.GetInt("Level_" + SceneManager.GetActiveScene().buildIndex + "_Completed", 0) == 1;
        if (isLevelCompleted) baseReward = 0;
        else baseReward = 5 + SceneManager.GetActiveScene().buildIndex;
    }

    // -------------------- CALCULATION --------------------

    private int CalculateTimeBonus()
    {
        int actualTime = GameManager.Instance.GetLevelPlayTime();
        if (actualTime >= minExpectedLevelPlayTime) return 0;

        float efficiency = 1f - (float)actualTime / minExpectedLevelPlayTime;
        int timeBonus = Mathf.RoundToInt(efficiency * maxTimeBonus);

        if (isLevelCompleted &&
            PlayerPrefs.GetInt("Level_" + SceneManager.GetActiveScene().buildIndex + "_TimeBonus", 0) > timeBonus)
        {
            return 0;
        }
        PlayerPrefs.SetInt("Level_" + SceneManager.GetActiveScene().buildIndex + "_TimeBonus", timeBonus);
        return timeBonus;
    }

    private int CalculateSnippetBonus()
    {
        int actualSnippets = SnippetsManagerUI.Instance.GetNumberOfSnippetsUsed();
        if (actualSnippets > minNumOfSnippets) return 0;

        float efficiency = 1.1f - (float)actualSnippets / minNumOfSnippets;
        int snippetBonus = Mathf.RoundToInt(efficiency * maxSnippetBonus);

        if (isLevelCompleted &&
            PlayerPrefs.GetInt("Level_" + SceneManager.GetActiveScene().buildIndex + "_SnippetBonus", 0) > snippetBonus)
        {
            return 0;
        }
        PlayerPrefs.SetInt("Level_" + SceneManager.GetActiveScene().buildIndex + "_SnippetBonus", snippetBonus);
        return snippetBonus;
    }

    public int GetLevelReward()
    {
        return baseReward + CalculateTimeBonus() + CalculateSnippetBonus();
    }

    // -------------------- REPORT --------------------

    public string GetLevelReport()
    {

        int timeBonus = CalculateTimeBonus();
        int snippetBonus = CalculateSnippetBonus();

        string tab = $"<pos={RIGHT_COLUMN}>";

        return
            $"\nBase reward{tab}  {baseReward}\n" +
            $"Time bonus{tab}  {FormatBonus(timeBonus)}\n" +
            $"Code efficiency{tab}  {FormatBonus(snippetBonus)}\n\n" +

            $"<color=#D4D4D4><b>Total{tab}  {EconomyManager.Instance.GetCurrentBalance()}</b></color>";
    }

    private string FormatBonus(int value)
    {
        return value > 0
            ? $"<color=#FFFFFF>{value}</color>"
            : $"<color=#666666>0</color>";
    }
}
