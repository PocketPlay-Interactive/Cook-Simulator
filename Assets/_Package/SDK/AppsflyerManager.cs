using System.Collections;
using System.Collections.Generic;
#if APPFLYER
using AppsFlyerSDK;
#endif
using UnityEngine;

public class AppsflyerManager : MonoSingletonGlobal<AppsflyerManager>
{
#if APPFLYER
    public string dev_key = "";
    public string app_id = "";
    public IEnumerator InitializedAppsflyer()
    {
        AppsFlyer.initSDK(dev_key, app_id);
        yield return null;
        AppsFlyer.startSDK();

        AppsFlyerAdRevenue.start();
    }

    public void AppsflyerLogAdRevenue(MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("AppsflyerLogAdRevenue " + adInfo.ToString());
        Dictionary<string, string> dic = new Dictionary<string, string>()
        {
            ["adUnitIdentifier"] = adInfo.AdUnitIdentifier,
            ["adFormat"] = adInfo.AdFormat,
            ["networkName"] = adInfo.NetworkName,
            ["networkPlacement"] = adInfo.NetworkPlacement,
            ["revenue"] = adInfo.Revenue.ToString("0.0000"),
            ["revenuePrecision"] = adInfo.RevenuePrecision,
            ["dspName"] = adInfo.DspName,
        };
        AppsFlyerAdRevenue.logAdRevenue("AppLovin", AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, adInfo.Revenue, "USD", dic);
    }

public void AdImpression(MaxSdkBase.AdInfo adInfo)
    {
        AppsFlyerSDK.AppsFlyer.sendEvent("af_ad_revenue", new Dictionary<string, string>()
        {
            ["ad_platform"] = "Applovin",
            ["ad_source"] = adInfo.NetworkName,
            ["ad_unit_name"] = adInfo.AdUnitIdentifier,
            ["ad_format"] = adInfo.AdFormat,
            ["value"] = adInfo.Revenue.ToString("0.0000"),
            ["currency"] = "USD",
        });
    }

    public void InterAfterShow()
    {
        AppsFlyerSDK.AppsFlyer.sendEvent("af_inters", null);
    }

    public void RewardAfterShow()
    {
        AppsFlyerSDK.AppsFlyer.sendEvent("af_reward", null);
    }

    public void PlayLevel(int level)
    {
        Debug.Log("appsflyer level play = " + (level));
        AppsFlyerSDK.AppsFlyer.sendEvent("play_level", new Dictionary<string, string>()
        {
            ["level"] = level.ToString(),
        });
    }

    public void LoseLevel(int level)
    {
        Debug.Log("appsflyer level lose = " + (level));
        AppsFlyerSDK.AppsFlyer.sendEvent("lose_level", new Dictionary<string, string>()
        {
            ["level"] = (level).ToString(),
        });
    }

    public void WinLevel(int level)
    {
        Debug.Log("appsflyer level win = " + (level));
        AppsFlyerSDK.AppsFlyer.sendEvent("win_level", new Dictionary<string, string>()
        {
            ["level"] = (level).ToString(),
        });
    }
#endif
}
