using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI :MonoBehaviour {

    [SerializeField] private Button closeButton;

    [Header("Animation")]
    [SerializeField] private Image bgImage;
    [SerializeField] private CanvasGroup contentCanvasGroup;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => {
            SoundManager.Instance.PlayUISound1();
            StartCoroutine(PlayInfoAnimation(false));
        });
        gameObject.SetActive(false);
    }
    private IEnumerator PlayInfoAnimation(bool fadeIn = true)
    {
        float duration = 0.15f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            if (fadeIn)
            {
                bgImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, t));
                contentCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            }
            else
            {
                bgImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, t));
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
        else
        {
            bgImage.color = new Color(0f, 0f, 0f, 1f);
            contentCanvasGroup.alpha = 1f;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(PlayInfoAnimation());
    }
}
