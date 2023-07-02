#if ADMOB_ENABLE
using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

public class AdmobManager : MonoSingletonGlobal<AdmobManager>
{

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/3419835294";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/5662855259";
#else
  private string _adUnitId = "unused";
#endif

    public AdStatus AppOpenStatus = AdStatus.Missing;

    public string AppId;
    //public string OpenAdsId;

    private AppOpenAd ad;
    private bool isShowingAd = false;

    //private bool IsAdAvailable
    //{
    //    get
    //    {
    //        return ad != null;
    //    }
    //}

    private float loadTime;
    private bool awake = false;
    private void Update()
    {
        if (awake)
            return;

        loadTime += Time.deltaTime;
        if(loadTime > 10)
        {
            awake = true;
            Manager.Instance.HideGlobalLoading();
            if (ShowAdCorotine != null)
                StopCoroutine(ShowAdCorotine);
        }
    }

    private Coroutine ShowAdCorotine;
    public IEnumerator InitializedAdmob()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => {
            Debug.Log("Admob " + initStatus);
            LoadAd();
        });
        yield return null;
        //ShowAdCorotine = StartCoroutine(ShowAd());
    }

    public void LoadAd()
    {
        if (AppOpenStatus == AdStatus.Loading)
            return;

        AppOpenStatus = AdStatus.Loading;

        // Clean up the old ad before loading a new one.
        if (ad != null)
        {
            ad.Destroy();
            ad = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(_adUnitId, ScreenOrientation.Portrait, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                AppOpenStatus = AdStatus.Ready;
                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                this.ad = ad;
                RegisterEventHandlers(ad);
            });
    }

    private Action _completeOpenAd;
    public void ShowOpenAd(Action completeAd)
    {
        _completeOpenAd = completeAd;
        StartCoroutine(ShowAd());
    }

    private IEnumerator ShowAd()
    {
        LoadAd();
        yield return new WaitUntil(() => AppOpenStatus == AdStatus.Ready);
        Manager.Instance.HideGlobalLoading();
        ShowAdIfAvailable();
    }    

    public void ShowAdIfAvailable()
    {
        if (AppOpenStatus != AdStatus.Ready || isShowingAd)
            return;

        if (ad == null)
            return;

        RegisterEventHandlers(ad);

        ad.Show();
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
            AppOpenStatus = AdStatus.Missing;
            if (_completeOpenAd != null)
                _completeOpenAd?.Invoke();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
            AppOpenStatus = AdStatus.Missing;
            if (_completeOpenAd != null)
                _completeOpenAd?.Invoke();
        };
    }

    private void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        UnityEngine.Debug.Log("App State is " + state);
        if (state == AppState.Foreground)
        {
            ShowAdIfAvailable();
        }
    }
}
#endif
