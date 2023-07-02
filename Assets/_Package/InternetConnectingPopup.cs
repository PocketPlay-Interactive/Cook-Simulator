using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InternetConnectingPopup : MonoBehaviour
{
    public Transform screen;
    public Transform popup;
    private bool isShow = false;


    private void Update()
    {
        NetworkCheckingAfter5Second();
    }

    private float time = 0;
    void NetworkCheckingAfter5Second()
    {
        time += Time.deltaTime;
        if(time > 5f)
        {
            time = 0;
            if (!isShow && Application.internetReachability == NetworkReachability.NotReachable)
            {
                isShow = true;
                popup.localScale = Vector3.one;
                screen.SetActive(true);
                popup.DOKill();
                popup.DOPunchScale(Vector3.one * 0.2f, 0.6f, 1, 1f).SetEase(Ease.OutBack);
            }
        }
    }

    public void Hide()
    {
        isShow = false;
        popup.DOKill();
        popup.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() => { screen.SetActive(false); });
    }

    //void CheckNetwork()
    //{
    //    switch (Application.internetReachability)
    //    {
    //        case NetworkReachability.NotReachable:
    //            Debug.Log("Không có kết nối mạng.");
    //            break;
    //        case NetworkReachability.ReachableViaCarrierDataNetwork:
    //            Debug.Log("Kết nối mạng qua 3G/4G.");
    //            break;
    //        case NetworkReachability.ReachableViaLocalAreaNetwork:
    //            Debug.Log("Kết nối mạng qua Wi-Fi.");
    //            break;
    //    }
    //}
}
