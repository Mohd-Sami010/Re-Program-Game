using System;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;

public class AdManager :MonoBehaviour {
    public static AdManager Instance { get; private set; }

    private RewardedAd rewardedAd;
    private bool isLoadingAd;

    // TEST Ad Unit ID (Android)
    private const string REWARDED_AD_UNIT_ID = "ca-app-pub-7670604770877714/8645034863"; //ca-app-pub-7670604770877714/8645034863

    private Action<bool> onAdResultCallback;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        MobileAds.Initialize(initStatus => {
            Debug.Log("AdMob Initialized");
            LoadRewardedAd();
        });
    }
    public bool IsAdReady()
    {
        return rewardedAd != null && rewardedAd.CanShowAd();
    }

    // -------------------- LOAD --------------------
    private void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        AdRequest adRequest = new AdRequest();

        RewardedAd.Load(REWARDED_AD_UNIT_ID, adRequest,
            (RewardedAd ad, LoadAdError error) => {
                if (error != null)
                {
                    Debug.LogError("Rewarded Ad failed to load: " + error);
                    return;
                }

                rewardedAd = ad;
                RegisterRewardedEvents(ad);
                Debug.Log("Rewarded Ad Loaded");
            });
    }

    // -------------------- SHOW --------------------
    public void ShowRewardedAdWithWait(float maxWaitSeconds, System.Action<bool> callback)
    {
        if (IsAdReady())
        {
            ShowRewardedAd(callback);
            return;
        }

        StartCoroutine(WaitAndShowAd(maxWaitSeconds, callback));
    }

    public void ShowRewardedAd(Action<bool> resultCallback)
    {
        if (rewardedAd == null || !rewardedAd.CanShowAd())
        {
            Debug.LogWarning("Rewarded Ad not ready");
            resultCallback?.Invoke(false);
            return;
        }

        onAdResultCallback = resultCallback;

        rewardedAd.Show(reward => {
            // This ONLY fires if user watched completely
            Debug.Log("User earned reward");
            onAdResultCallback?.Invoke(true);
            onAdResultCallback = null;
        });
    }
    private IEnumerator WaitAndShowAd(float maxWait, System.Action<bool> callback)
    {
        float timer = 0f;

        while (timer < maxWait)
        {
            if (IsAdReady())
            {
                ShowRewardedAd(callback);
                yield break;
            }

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Failed to load
        callback?.Invoke(false);
    }


    // -------------------- EVENTS --------------------
    private void RegisterRewardedEvents(RewardedAd ad)
    {
        ad.OnAdFullScreenContentOpened += () => {
            Debug.Log("Ad opened");
        };

        ad.OnAdFullScreenContentClosed += () => {
            Debug.Log("Ad closed");
            LoadRewardedAd(); // preload next
        };

        ad.OnAdFullScreenContentFailed += error => {
            Debug.LogError("Ad failed to show: " + error);
            onAdResultCallback?.Invoke(false);
            onAdResultCallback = null;
            LoadRewardedAd();
        };
    }
}
