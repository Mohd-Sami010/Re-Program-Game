using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI :MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI playButtonTextMesh;
    [SerializeField] private Button viewCodeButton;
    [SerializeField] private Button levelSelectButton;
    [SerializeField] private GameObject levelSelectorUIObject;
    [SerializeField] private GameObject loadingUIObject;

    private void Start()
    {
        int levelToLoad = PlayerPrefs.GetInt("LevelToContinue", 1);

        if (levelToLoad == 1) playButtonTextMesh.text = "Play";
        else playButtonTextMesh.text = "Continue";

        loadingUIObject.SetActive(false);
        playButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            loadingUIObject.SetActive(true);
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);
            
        });
        levelSelectButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            levelSelectorUIObject.SetActive(true);
            gameObject.SetActive(false);
        });
        viewCodeButton.onClick.AddListener(() => {
            Application.OpenURL("https://github.com/Mohd-Sami010/Re-Program-Game/tree/main/Assets/Scripts");
        });
    }

}
