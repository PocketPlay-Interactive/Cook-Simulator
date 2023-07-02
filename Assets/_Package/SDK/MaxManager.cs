using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdStatus
{
    Missing,
    Loading,
    Ready
}

public class MaxManager : MonoSingletonGlobal<MaxManager>
{
    public string MaxSDK = "4-9EtTDRmLxbkDtFixuO-TkgcBchq4RK-86TKS_TCp10pwVuhTQ2pyIC5ebz53Uq24gsZFq1uoLoACbttA3TxH";
    public string MaxReward = "db06a4e759e31431";
    public string MaxInter = "59eb20654a619314";
    public string MaxBanner = "87c72c5be5217e56";

    private Action Interstitial_Callback;
    private Action Interstitial_Callback_Open;
    private Action Interstitial_Callback_Recive;


    private Action Reward_Success_Callback;
    private Action Reward_Success_Callback_Open;
    private Action Reward_Fail_Callback;
    private Action Reward_Recive_Callback;

    public AdStatus BannerAdStatus = AdStatus.Missing;
    public AdStatus InterAdStatus = AdStatus.Missing;
    public AdStatus RewardAdStatus = AdStatus.Missing;

    public GameObject LoadingAd;

    public IEnumerator InitializedAppLovin()
    {
        yield return null;
#if MAX
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads
#if !AD_ENABLE
            InitializeBannerAds();
            InitializeInterstitialAds();
            InitializeRewardedAds();
#endif
            // Show Mediation Debugger
            MaxSdk.ShowMediationDebugger();
        };

        MaxSdk.SetSdkKey(MaxSDK);
        MaxSdk.InitializeSdk();
        yield return WaitForSecondCache.WAIT_TIME_ONE;

        //_initialized = true;
#endif
    }

    public void InitializeBannerAds()
    {
#if APPLOVIN_ENABLE
        // Banners are automatically sized to 320?50 on phones and 728?90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(MaxBanner, MaxSdkBase.BannerPosition.TopCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(MaxBanner, Color.black);

        //Start auto-refresh for a banner ad with the following code:
        //MaxSdk.StartBannerAutoRefresh(MaxBanner);

        //There may be cases when you would like to stop auto-refresh, for instance, if you want to manually refresh banner ads. To stop auto-refresh for a banner ad, use the following code:
        //MaxSdk.StopBannerAutoRefresh(MaxBanner);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        //MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        //MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        //MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

        //Load banner
        LoadBanner();
        HideBanner();
#endif
    }

    public void LoadBanner()
    {
#if APPLOVIN_ENABLE
        if (BannerAdStatus == AdStatus.Missing)
        {
            //Debug.Log("max sdk load banner " + DateTime.Now);
            BannerAdStatus = AdStatus.Loading;
            MaxSdk.LoadBanner(MaxBanner);
        }
#endif
    }

    public void ShowBanner()
    {
#if APPLOVIN_ENABLE
        MaxSdk.ShowBanner(MaxBanner);
#endif
    }

    public void HideBanner()
    {
#if APPLOVIN_ENABLE
        MaxSdk.HideBanner(MaxBanner);
#endif
    }

#if APPLOVIN_ENABLE
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        BannerAdStatus = AdStatus.Ready;
        bannerTimer = 0;
        Debug.Log("OnBannerAdLoadedEvent");
    }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        BannerAdStatus = AdStatus.Missing;
        Debug.Log("OnBannerAdLoadFailedEvent");
    }
#endif

    public void InitializeInterstitialAds()
    {
#if APPLOVIN_ENABLE
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        //MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        //MaxSdkCallbacks.Interstitial.On
#endif

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
#if APPLOVIN_ENABLE
        //if (!MaxSdk.IsInterstitialReady(MaxInter) && InterAdStatus == AdStatus.Missing)
        if (InterAdStatus == AdStatus.Missing)
        {
            //Debug.Log("max sdk load inter " + DateTime.Now);
            InterAdStatus = AdStatus.Loading;
            MaxSdk.LoadInterstitial(MaxInter);
        }
        else
        {
            InterAdStatus = AdStatus.Ready;
        }
#endif
    }

#if APPLOVIN_ENABLE
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
        // Reset retry attempt
        InterAdStatus = AdStatus.Ready;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        InterAdStatus = AdStatus.Missing;
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Interstitial_Callback_Open?.Invoke();
        Interstitial_Callback_Recive?.Invoke();
        Debug.Log("OnInterstitialDisplayedEvent");
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        InterAdStatus = AdStatus.Missing;
        Interstitial_Callback?.Invoke();
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        InterAdStatus = AdStatus.Missing;
        Interstitial_Callback?.Invoke();
    }
#endif

    public void ShowInter(Action callback, Action onOpen = null, Action recive = null)
    {
#if AD_ENABLE
        CoroutineUtils.PlayManyCoroutine(0.0f, 0.25f, () => onOpen?.Invoke(), () => callback?.Invoke());
#elif APPLOVIN_ENABLE
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (onOpen != null)
                onOpen?.Invoke();
            callback?.Invoke();
        }
        else if (InterAdStatus == AdStatus.Loading || InterAdStatus == AdStatus.Loading)
        {
            if (onOpen != null)
                onOpen?.Invoke();
            callback?.Invoke();
        }
        else
        {
            LoadingAd.SetActive(true);
            Interstitial_Callback_Open = () =>
            {
                LoadingAd.SetActive(false);
                if (onOpen != null)
                    onOpen?.Invoke();
            };
            Interstitial_Callback = () =>
            {
                LoadingAd.SetActive(false);
                callback?.Invoke();
            };
            Interstitial_Callback_Recive = () =>
            {
                recive?.Invoke();
            };
            MaxSdk.ShowInterstitial(MaxInter);
        }
#else
        CoroutineUtils.PlayManyCoroutine(0.0f, 0.25f, () => onOpen?.Invoke(), () => callback?.Invoke());
#endif
    }

    public void InitializeRewardedAds()
    {
#if APPLOVIN_ENABLE
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        //MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        //MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
#endif
        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
#if APPLOVIN_ENABLE
        //        if (!MaxSdk.IsRewardedAdReady(MaxReward) && RewardAdStatus == AdStatus.Missing)
        if (RewardAdStatus == AdStatus.Missing)
        {
            //Debug.Log("max sdk load reward " + DateTime.Now);
            RewardAdStatus = AdStatus.Loading;
            MaxSdk.LoadRewardedAd(MaxReward);
        }
        else
        {
            RewardAdStatus = AdStatus.Ready;
        }
#endif
    }

#if APPLOVIN_ENABLE
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        RewardAdStatus = AdStatus.Ready;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        RewardAdStatus = AdStatus.Missing;
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Reward_Success_Callback_Open?.Invoke();
        Reward_Recive_Callback?.Invoke();
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        RewardAdStatus = AdStatus.Missing;
        Reward_Fail_Callback?.Invoke();
    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        RewardAdStatus = AdStatus.Missing;
        Reward_Success_Callback?.Invoke();
    }
#endif

    public void ShowReward(Action callback_success, Action callback_fail = null, Action onOpen = null, Action recive = null)
    {
#if AD_ENABLE
        CoroutineUtils.PlayManyCoroutine(0.0f, 0.25f, () => onOpen?.Invoke(), () => callback_success?.Invoke());
#elif APPLOVIN_ENABLE
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (onOpen != null)
                onOpen?.Invoke();
            if (callback_fail != null)
                callback_fail?.Invoke();
        }
        else if (RewardAdStatus == AdStatus.Loading || RewardAdStatus == AdStatus.Loading)
        {
            if (onOpen != null)
                onOpen?.Invoke();
            if (callback_fail != null)
                callback_fail?.Invoke();
        }
        else
        {
            LoadingAd.SetActive(true);
            Reward_Success_Callback_Open = () =>
            {
                LoadingAd.SetActive(false);
                if (onOpen != null)
                    onOpen?.Invoke();
            };
            Reward_Success_Callback = () =>
            {
                LoadingAd.SetActive(false);
                callback_success?.Invoke();
            };
            Reward_Fail_Callback = () =>
            {
                LoadingAd.SetActive(false);
                if (callback_fail != null)
                    callback_fail?.Invoke();
            };
            Reward_Recive_Callback = () =>
            {
                recive?.Invoke();
            };
            MaxSdk.ShowRewardedAd(MaxReward);
        }
#else
        CoroutineUtils.PlayManyCoroutine(0.0f, 0.25f, () => onOpen?.Invoke(), () => callback_success?.Invoke());
#endif
    }

    private float timer = 0;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 3f)
        {
            timer = 0;
            if (BannerAdStatus == AdStatus.Missing)
                LoadBanner();

            if (InterAdStatus == AdStatus.Missing)
                LoadInterstitial();

            if (RewardAdStatus == AdStatus.Missing)
                LoadRewardedAd();
#if !UNITY_EDITOR
            Debug.Log($"max sdk banner {BannerAdStatus} inter {InterAdStatus} reward {RewardAdStatus}");
#endif
        }

        AutoFreshBanner();
    }

    private float bannerTimer = 0;
    private void AutoFreshBanner()
    {
        bannerTimer += Time.deltaTime;
        if(bannerTimer >= 30f)
        {
            bannerTimer = 0;
            BannerAdStatus = AdStatus.Missing;
        }
    }

#if MAX
    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        FirebaseManager.Instance.AdImpression(adInfo);
        AppsflyerManager.Instance.AdImpression(adInfo);
        AppsflyerManager.Instance.AppsflyerLogAdRevenue(adInfo);
    }
#endif
}
