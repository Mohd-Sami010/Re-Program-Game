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
        //string diffrenceInTimeStr =
        //// Color a specific word using a named color tag
        //myText.text = "I want to change the <color=\"blue\">color</color> of this word.";

        //// Or use a hexadecimal color value
        //// myText.text = "I want this part to be <color=#FF0000>red</color>.";
        string levelReport = 
            $"Level: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex}\n" +
            $"Time Taken: {GameManager.Instance.GetLevelPlayTimeString()}\n"+
            $"Code blocks used: {SnippetsManagerUI.Instance.GetNumberOfSnippetsUsed()}";
        return levelReport;
    }

}
