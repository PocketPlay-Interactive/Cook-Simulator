using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyController : MonoSingleton<CurrencyController>
{
    // Gold
    public void AddGold(int _valueGold)
    {
        DOTween.To(() => RuntimeStorageData.Player.Gold, x => RuntimeStorageData.Player.Gold = x, RuntimeStorageData.Player.Gold + _valueGold, 0.75f);
    }

    public void AddGold(int _valueGold, Action _callback)
    {
        DOTween.To(() => RuntimeStorageData.Player.Gold, x => RuntimeStorageData.Player.Gold = x, RuntimeStorageData.Player.Gold + _valueGold, 0.75f).OnComplete(() => { _callback?.Invoke(); });
    }

    public void SpendGold(int _valueGold)
    {
        DOTween.To(() => RuntimeStorageData.Player.Gold, x => RuntimeStorageData.Player.Gold = x, RuntimeStorageData.Player.Gold - _valueGold, 0.75f);
    }

    public bool EnoughGold(int _valueGold)
    {
        if (RuntimeStorageData.Player.Gold >= _valueGold)
            return true;
        return false;     
    }

    public int GetGold() { return RuntimeStorageData.Player.Gold; }
    public string GetGoldString() { return RuntimeStorageData.Player.Gold.ToString(); }

    public int CaculateVictoryGold()
    {
        return UnityEngine.Random.Range(100, 130);
    }

    public int CaculateLoseGold()
    {
        return UnityEngine.Random.Range(50, 60);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            AddGold(100);
        }
    }
#endif

    // Gem
    // Cash
}
