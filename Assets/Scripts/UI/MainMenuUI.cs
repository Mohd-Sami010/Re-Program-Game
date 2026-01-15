using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI :MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI playButtonTextMesh;
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button infoButton;
    [SerializeField] private GameObject levelSelectorUIObject;
    [SerializeField] private GameObject settingsUIObject;
    [SerializeField] private GameObject infoUIObject;
    [SerializeField] private LoadingUI loadingUIObject;

    private void Start()
    {
        int levelToLoad = PlayerPrefs.GetInt("LevelToContinue", 1);

        if (levelToLoad == 1) playButtonTextMesh.text = "Play";
        else playButtonTextMesh.text = "Continue";

        playButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelToLoad);
            loadingUIObject.EnableLoadingUI();
            
        });
        levelSelectButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            levelSelectorUIObject.SetActive(true);
        });
        settingsButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            settingsUIObject.SetActive(true);
        });
        infoButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            infoUIObject.SetActive(true);
        });
    }

}
