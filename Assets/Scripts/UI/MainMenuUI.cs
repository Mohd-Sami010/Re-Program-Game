using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI :MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject loadingUIObject;

    private void Start()
    {
        loadingUIObject.SetActive(false);
        playButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            loadingUIObject.SetActive(true);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            
        });
        quitButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            Application.Quit();
        });
    }

}
