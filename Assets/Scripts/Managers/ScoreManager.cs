using UnityEngine;

public class ScoreManager :MonoBehaviour {
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int minExpectedLevelPlayTime = 30;
    [SerializeField] private int minNumOfSnippets = 5;

    private void Awake()
    {
        Instance = this;
    }
    public int GetLevelReward()
    {
        int levelScore = 2 + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int playTimeScore = minExpectedLevelPlayTime * 2 / GameManager.Instance.GetLevelPlayTime();
        int numOfSnippetsScore = minNumOfSnippets * 2 / SnippetsManagerUI.Instance.GetNumberOfSnippetsUsed();

        return (levelScore + playTimeScore + numOfSnippetsScore);
    }
    public string GetLevelReport()
    {
        int diffrenceInTime = GameManager.Instance.GetLevelPlayTime() - minExpectedLevelPlayTime;

        string timeDifferenceStr = $"Time taken: {GameManager.Instance.GetLevelPlayTimeString()} ";
        if (diffrenceInTime > 0) timeDifferenceStr += $"<color=\"red\">+{diffrenceInTime}s</color>";
        else timeDifferenceStr += $"<color=\"green\">{diffrenceInTime}s</color>";

        int diffrenceInNumOfSnippets = SnippetsManagerUI.Instance.GetNumberOfSnippetsUsed() - minNumOfSnippets;

        string numOfSnippetsDifferenceStr = $"Code blockes used: {SnippetsManagerUI.Instance.GetNumberOfSnippetsUsed()} ";
        if (diffrenceInNumOfSnippets > 0) numOfSnippetsDifferenceStr += $"<color=\"red\">+{diffrenceInNumOfSnippets}</color>";
        else numOfSnippetsDifferenceStr += $"<color=\"green\">{diffrenceInNumOfSnippets}</color>";


        //string diffrenceInTimeStr =
        //// Color a specific word using a named color tag
        //myText.text = "I want to change the <color=\"blue\">color</color> of this word.";

        //// Or use a hexadecimal color value
        //// myText.text = "I want this part to be <color=#FF0000>red</color>.";
        string levelReport =
                $"Level: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex}\n" +
                $"{timeDifferenceStr}\n" +
                $"{numOfSnippetsDifferenceStr}\n" +
                $"<color=#FFD200>Total reward: {GetLevelReward()}{EconomyManager.Instance.GetCurrencyName()}</color>";
        return levelReport;
    }

}
