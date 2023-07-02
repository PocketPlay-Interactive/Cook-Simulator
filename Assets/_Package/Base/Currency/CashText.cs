using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashText : MonoBehaviour
{
    private int Cash = -1;
    private Text _textCash;

    private void Start()
    {
        _textCash = this.GetComponent<Text>();
    }

    private void Update()
    {
        if (RuntimeStorageData.IsReady)
        {
            if (RuntimeStorageData.Player.Cash != Cash)
            {
                Cash = RuntimeStorageData.Player.Cash;
                _textCash.text = Cash.ToString();
            }
        }
        else
        {
            Cash = 0;
            _textCash.text = Cash.ToString();
        }
    }
}
