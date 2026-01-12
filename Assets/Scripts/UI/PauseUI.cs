using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI :MonoBehaviour {

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsUi;

    [Header("Animation")]
    [SerializeField] private Image bgImage;
    [SerializeField] private CanvasGroup contentCanvasGroup;

    private void Start()
    {
        resumeButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.TogglePauseGame();
            StartCoroutine(PlayPauseAnimation(false));
        });
        menuButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            GameManager.Instance.LoadMainMenu();
        });
        settingsButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            settingsUi.SetActive(true);
            //gameObject.SetActive(false);
        });
        GameManager.Instance.OnGamePause += GameManager_OnGamePause;
        gameObject.SetActive(false);

    }
    private void GameManager_OnGamePause(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        StartCoroutine(PlayPauseAnimation(true));
    }
    private IEnumerator PlayPauseAnimation(bool fadeIn = true)
    {
        float duration = 0.15f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            if (fadeIn)
            {
                bgImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 0.94f, t));
                contentCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            }
            else
            {
                bgImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(0.94f, 0f, t));
                contentCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            }
            yield return null;
        }
        if (!fadeIn)
        {
            bgImage.color = new Color(0f, 0f, 0f, 0f);
            contentCanvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }
        else {
            bgImage.color = new Color(0f, 0f, 0f, 0.94f);
            contentCanvasGroup.alpha = 1f;
        }
    }
}
