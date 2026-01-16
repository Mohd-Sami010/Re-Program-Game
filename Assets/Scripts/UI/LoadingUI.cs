using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI :MonoBehaviour {


    [Header("Animation")]
    [SerializeField] private Image bgImage;
    [SerializeField] private CanvasGroup contentCanvasGroup;

    private void Start()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnLoadScene += EnableLoadingUI;
        StartCoroutine(PlayLoadingAnimation(false));
    }

    public void EnableLoadingUI()
    {
        gameObject.SetActive(true);
        StartCoroutine(PlayLoadingAnimation());
    }
    private IEnumerator PlayLoadingAnimation(bool fadeIn = true)
    {
        float duration = 0.07f;
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
}
