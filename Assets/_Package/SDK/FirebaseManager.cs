using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#if FIREBASE
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
#endif
using UnityEngine;

public class FirebaseManager : MonoSingletonGlobal<FirebaseManager>
{
#if FIREBASE
    private bool IsFirebaseInitialized = false;

    [SerializeField] private bool remoteconfig_app_open = false;
    public bool IsAppOpenAd()
    {
        if (!remoteconfig_app_open)
            return false;

        var session = PrefManager.GetInt("session");
        if (session < remoteconfig_app_open_session)
            return false;

        return true;
    }
    [SerializeField] private int remoteconfig_app_open_session = 2;
    [SerializeField] private bool remoteconfig_ingame_bar_inter = true;
    [SerializeField] private int remoteconfig_ingame_delay_inter_time = 30;

    public void ResetIngameDelayTime(float time)
    {
        ingame_delay_inter_timer = time;
    }

    private float ingame_delay_inter_timer = 0;
    public bool IsIngameBarAdReadyAfterTime(Action CALLBACK)
    {
        if (RuntimeStorageData.Player.TotalGames < 1)
            return false;

        if (!remoteconfig_ingame_bar_inter)
            return false;

        if (ingame_delay_inter_timer < remoteconfig_ingame_delay_inter_time)
            return false;

        ingame_delay_inter_timer = 0;
        return true;
    }

    private void Update()
    {
        if (ingame_delay_inter_timer < remoteconfig_ingame_delay_inter_time)
            ingame_delay_inter_timer += Time.deltaTime;
    }

    public IEnumerator InitializedFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Initializer maybe
                var app = FirebaseApp.DefaultInstance;
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                // Finish initializer
                Debug.Log("Firebase initialized");
                IsFirebaseInitialized = true;
                InitializedFirebaseRemoteConfig();
            }
            else
            {
                IsFirebaseInitialized = true;
                Debug.LogError(string.Format("Dependency error: {0}", dependencyStatus)); // Firebase Unity SDK is not safe to use here.
            }
        });
        yield return null;
    }

    //public IEnumerator InitializedFirebaseMessaging()
    //{
    //    yield return new WaitUntil(() => IsFirebaseInitialized);
    //    Debug.Log("Firebase Messaging initialized");
    //    Firebase.Messaging.FirebaseMessaging.TokenReceived += FirebaseMessaging_TokenReceived;
    //    Firebase.Messaging.FirebaseMessaging.MessageReceived += FirebaseMessaging_MessageReceived;
    //}

    // Start a fetch request.
    // FetchAsync only fetches new data if the current data is older than the provided
    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    // By default the timespan is 12 hours, and for production apps, this is a good
    // number. For this example though, it's set to a timespan of zero, so that
    // changes in the console will always show up immediately.
    public Task InitializedFirebaseRemoteConfig()
    {
        Debug.Log("Firebase Fetching data...");
        System.Threading.Tasks.Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        Debug.Log("Firebase Fetching data.......................");
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Firebase Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"Firebase {nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task => {
                //var test = remoteConfig.GetValue("test").BooleanValue;
                remoteconfig_app_open = remoteConfig.GetValue("remoteconfig_app_open").BooleanValue;
                Debug.Log("Firebase remoteconfig_app_open " + remoteconfig_app_open);
                remoteconfig_app_open_session = (int)remoteConfig.GetValue("remoteconfig_app_open_session").DoubleValue;
                Debug.Log("Firebase remoteconfig_app_open_session " + remoteconfig_app_open_session);
                remoteconfig_ingame_bar_inter = remoteConfig.GetValue("remoteconfig_ingame_bar_inter").BooleanValue;
                Debug.Log("Firebase remoteconfig_ingame_bar_inter " + remoteconfig_ingame_bar_inter);
                remoteconfig_ingame_delay_inter_time = (int)remoteConfig.GetValue("remoteconfig_ingame_delay_inter_time").DoubleValue;
                Debug.Log("Firebase remoteconfig_ingame_delay_inter_time " + remoteconfig_ingame_delay_inter_time);
                //Debug.Log(test);
                Debug.Log($"Firebase Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
            });
    }

    private void FirebaseMessaging_MessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }

    private void FirebaseMessaging_TokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + e.Token);
    }

    public int ads_inter_click = 0;
    public int ads_inter_show = 0;

    public void AdsInterShow(string position)
    {
        if (!IsFirebaseInitialized)
            return;

        ads_inter_show += 1;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("placement", position),
        };
        FirebaseAnalytics.LogEvent("ads_inter_show", parameters);
    }

    public void AdsInterClick(string position)
    {
        if (!IsFirebaseInitialized)
            return;

        ads_inter_click += 1;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("placement", position),
        };
        FirebaseAnalytics.LogEvent("ads_inter_click", parameters);
    }

    public int ads_reward_click = 0;
    public int ads_reward_show = 0;

    public void AdsRewardShow(string position)
    {
        if (!IsFirebaseInitialized)
            return;

        ads_reward_show += 1;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("placement", position),
        };
        FirebaseAnalytics.LogEvent("ads_reward_show", parameters);
    }

    public void AdsRewardClick(string position)
    {
        if (!IsFirebaseInitialized)
            return;

        ads_reward_click += 1;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("placement", position),
        };
        FirebaseAnalytics.LogEvent("ads_reward_click", parameters);
    }

    public void TimeCheckPoint(string index)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("scene", "game"),
        };

        FirebaseAnalytics.LogEvent($"checkpoint_{index}", parameters);
        Debug.Log($"checkpoint_{index} at " + System.DateTime.Now);
    }

    public void LevelStartCheckPoint(int level)
    {
        if (!IsFirebaseInitialized)
            return;

        var _strLevel = level < 10 ? $"0{level}" : $"{level}";
        FirebaseAnalytics.LogEvent($"checkpoint_level_{_strLevel}_start");
        Debug.Log($"checkpoint_level_{_strLevel}_start at " + System.DateTime.Now);
    }

    public void LevelStart(int play_time, string theme)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
          new Parameter("play_time", play_time),
          new Parameter("theme", theme),
        };

        FirebaseAnalytics.LogEvent("game_start", parameters);
        Debug.Log($"game_start at " + System.DateTime.Now);
    }    

    public void LevelEndCheckPoint(int level)
    {
        if (!IsFirebaseInitialized)
            return;

        var _strLevel = level - 1 < 10 ? $"0{level - 1}" : $"{level - 1}";
        FirebaseAnalytics.LogEvent($"checkpoint_level_{_strLevel}_end");
        Debug.Log($"checkpoint_level_{_strLevel}_end at " + System.DateTime.Now);
    }

    public void LevelEnd(int play_time, float duration, int skin, int hair, int eyeshadow, int eyes, int eyebrow, int eyelash, int lipstick, int top, int bottom, int dress, int necklace, int earings, int bag, int shoes, int access, int glasses)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
          new Parameter("play_time", play_time),
          new Parameter("duration", duration),
          new Parameter("skin", skin),
          new Parameter("hair", hair),
          new Parameter("eyeshadow", eyeshadow),
          new Parameter("eyes", eyes),
          new Parameter("eyebrow", eyebrow),
          new Parameter("eyelash", eyelash),
          new Parameter("lipstick", lipstick),
          new Parameter("top", top),
          new Parameter("bottom", bottom),
          new Parameter("dress", dress),
          new Parameter("necklace", necklace),
          new Parameter("earings", earings),
          new Parameter("bag", bag),
          new Parameter("shoes", shoes),
          new Parameter("access", access),
          new Parameter("glasses", glasses),
        };

        FirebaseAnalytics.LogEvent("game_end", parameters);
        Debug.Log($"game_end at " + System.DateTime.Now);
    }    

    public void CVPlayTime(string time)
    {
        if (!IsFirebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent($"cv_play_{time}min");
        Debug.Log($"cv_play_{time}min at " + System.DateTime.Now);
    }

    public void CVRetention(string day)
    {
        if (!IsFirebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent($"cv_retention_d{day}");
        Debug.Log($"cv_retention_d{day} at " + System.DateTime.Now);
    }

    public void SubmitCollecttion(string paramValue)
    {
        if (!IsFirebaseInitialized)
            return;
        var session = PrefManager.GetInt("session");
        Parameter[] parameters = {
          new Parameter("item_list", $"{session} {paramValue}"),
        };

        FirebaseAnalytics.LogEvent("submit_skin", parameters);
    }

    public void BuyWithCash(string id)
    {
        if (!IsFirebaseInitialized)
            return;

        var session = PrefManager.GetInt("session");

        Parameter[] parameters = {
          new Parameter("item_list", $"{session} {id}"),
        };

        FirebaseAnalytics.LogEvent("purchase_cash", parameters);
    }

    public void BuyWithGem(string id)
    {
        if (!IsFirebaseInitialized)
            return;

        var session = PrefManager.GetInt("session");

        Parameter[] parameters = {
          new Parameter("item_list", $"{session} {id}"),
        };

        FirebaseAnalytics.LogEvent("purchase_gem", parameters);
    }

    public void InterAfterShow()
    {
        if (!IsFirebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent("af_inters");
    }

    public void InterCall()
    {
        if (!IsFirebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent("inter_attempt");
    }

    public void RewardAfterShow(string reward_type, string item_list, string furniture_list)
    {
        if (!IsFirebaseInitialized)
            return;

        var session = PrefManager.GetInt("session");

        Parameter[] parameters = {
          new Parameter("reward_type", $"{reward_type}"),
          new Parameter("item_list", $"{session} {item_list}"),
          new Parameter("furniture_list", $"{session} {furniture_list}"),
        };

        FirebaseAnalytics.LogEvent("af_reward", parameters);
    }

    public void RewardCall()
    {
        FirebaseAnalytics.LogEvent("reward_attempt");
    }

    public void AdImpression(MaxSdkBase.AdInfo impressionData)
    {
        if (!IsFirebaseInitialized)
            return;

        double revenue = impressionData.Revenue;
        var impressionParameters = new[] {
            new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
            new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
            new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
            new Firebase.Analytics.Parameter("ad_format", impressionData.AdFormat),
            new Firebase.Analytics.Parameter("value", revenue),
            new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
            };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    }
#endif
}
