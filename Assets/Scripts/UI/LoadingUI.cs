using System.Collections;
using UnityEngine;

public class LoadingUI :MonoBehaviour {

    private void Start()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnLoadScene += EnableLoadingUI;
        StartCoroutine(PlayFadeOutAnim());
    }

    public void EnableLoadingUI()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("FadeIn");
    }
    private IEnumerator PlayFadeOutAnim()
    {
        GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(0.25f);
        gameObject.SetActive(false);
    }
}
