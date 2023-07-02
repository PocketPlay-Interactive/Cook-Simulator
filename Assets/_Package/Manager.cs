using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoSingletonGlobal<Manager>
{
    private bool Session1;
    protected override void Awake()
    {
        base.Awake();
        var IsCreate = PrefManager.GetBool("Game Create", false);
        if(IsCreate)
        {
            PrefManager.SetBool("Game Create", true);
            RuntimeStorageData.CreateData();
        }
        else
        {
            RuntimeStorageData.ReadData();
        }

        Session1 = PrefManager.GetBool("Session 1", false);
        if (!Session1)
            PrefManager.SetBool("Session 1", true);

        ShowGlobalLoading();
    }

    //public bool IsOpen = true;
    public bool IsOpenAll = false;
    public bool IsInitializedComplete = false;

    private IEnumerator Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer && PlayerPrefs.GetInt("ATTShowed", 0) == 0 && UnityATTPlugin.Instance.IsIOS14AndAbove())
        {
            //AnalysticManager.Instance.ATTShow();
            UnityATTPlugin.Instance.ShowATTRequest((action) =>
            {
                //if (action == ATTStatus.Authorized)
                    //AnalysticManager.Instance.ATTSuccess();
            });
            PlayerPrefs.SetInt("ATTShowed", 1);
        }
#if UNITY_IOS
        // Set the flag as true 
        AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
#endif

        ShowGlobalLoading();

        yield return null;
#if FIREBASE_ENABLE
        yield return FirebaseManager.Instance.InitializedFirebase();
        //yield return FirebaseManager.Instance.InitializedFirebaseMessaging();
#endif
#if APPFLYER
        yield return AppsflyerManager.Instance.InitializedAppsflyer();
#endif
#if ADMOB_ENABLE
        yield return AdmobManager.Instance.InitializedAdmob();
#endif
#if APPLOVIN_ENABLE
        yield return MaxManager.Instance.InitializedAppLovin();
#endif
        IsInitializedComplete = true;
#if FIREBASE
        var session = PrefManager.GetInt("session");
        session += 1;
        PrefManager.SetInt("session", session);

        //caculate day;
        var _caculateDayNow = PrefManager.GetInt("retentionTime");
        if(_caculateDayNow != StaticTimerHelper.CurrentTimeDayInDay())
        {
            Debug.Log($"{_caculateDayNow} - {StaticTimerHelper.CurrentTimeDayInDay()} is next day");
            _caculateDayNow = StaticTimerHelper.CurrentTimeDayInDay();
            PrefManager.SetInt("retentionTime", _caculateDayNow);
            var _caculateRetention = PrefManager.GetInt("retention", 0);
            FirebaseManager.Instance.CVRetention(_caculateRetention.ToString());
            _caculateRetention += 1;
            PrefManager.SetInt("retention", _caculateRetention);
        }
#endif

        yield return new WaitUntil(() => RuntimeStorageData.IsReady);
        UnityEngine.SceneManagement.SceneManager.LoadScene($"Game");
    }

#if FIREBASE
    private float timerCheckpoint = 0;
    private int index = 0;

    private float timerCV = 0;
    private bool cv300 = false;
    private bool cv480 = false;
    private bool cv780 = false;

    private int min1 = 1000, max1 = 0, min2 = 1000, max2 = 0;
#endif
    private void Update()
    {
        if (!IsInitializedComplete)
            return;

        AutoHideLoadingAfter30Second();
#if FIREBASE
        if (session_1)
        {
            timerCheckpoint += Time.deltaTime;
            if (timerCheckpoint > 30)
            {
                timerCheckpoint = 0;
                index += 1;
                FirebaseManager.Instance.TimeCheckPoint(index < 10 ? $"0{index}" : $"{index}");
                if (index >= 20)
                    session_1 = false;
            }
        }

        timerCV += Time.deltaTime;
        if(!cv300 && timerCV >= 300)
        {
            cv300 = true;
            FirebaseManager.Instance.CVPlayTime("05");
        }
        if(!cv480 && timerCV > 480)
        {
            cv480 = true;
            FirebaseManager.Instance.CVPlayTime("08");
        }
        if (!cv780 && timerCV > 780)
        {
            cv780 = true;
            FirebaseManager.Instance.CVPlayTime("13");
        }

        if (_duration)
            _timer += Time.deltaTime;
#endif
    }


    private bool _duration = false;
    private float _timer;
    public void GameDurationStart()
    {
        _duration = true;
        _timer = 0;
    }

    public float GameDurationEnd()
    {
        _duration = false;
        return _timer;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            RuntimeStorageData.SaveAllData();
    }

    private void OnApplicationQuit()
    {
        RuntimeStorageData.SaveAllData();
    }

    private bool isLoading = false;
    private float timeLoading = 0;
    private void AutoHideLoadingAfter30Second()
    {
        if(isLoading)
        {
            timeLoading += Time.deltaTime;
            if (timeLoading > 5f)
                HideGlobalLoading();
        }
    }
    [SerializeField] LoadingScreen LoadingGlobal;
    public void ShowGlobalLoading()
    {
        if (LoadingGlobal != null)
            LoadingGlobal.Show();
        isLoading = true;
        timeLoading = 0;
    }

    public void HideGlobalLoading()
    {
        if (LoadingGlobal != null)
            LoadingGlobal.Hide();
        isLoading = false;
    }
}
