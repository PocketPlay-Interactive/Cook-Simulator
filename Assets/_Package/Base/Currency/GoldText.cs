using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldText : MonoBehaviour
{
    private int Gold = -1;
    private Text _textGold;

    private void Start()
    {
        _textGold = this.GetComponent<Text>();
    }

    private void Update()
    {
        if (RuntimeStorageData.IsReady)
        {
            if (RuntimeStorageData.Player.Gold != Gold)
            {
                Gold = RuntimeStorageData.Player.Gold;
                _textGold.text = Gold.ToString();
            }
        }
        else
        {
            Gold = 0;
            _textGold.text = Gold.ToString();
        }
    }
}
