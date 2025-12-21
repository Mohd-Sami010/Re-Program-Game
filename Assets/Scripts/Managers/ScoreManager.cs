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
        int levelScore = 6 + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int playTimeScore = minExpectedLevelPlayTime * 5 / GameManager.Instance.GetLevelPlayTime();
        int numOfSnippetsScore = minNumOfSnippets * 5 / SnippetsManagerUI.Instance.GetNumberOfSnippetsUsed();

        return (levelScore + playTimeScore + numOfSnippetsScore);
    }

}
