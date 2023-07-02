using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemText : MonoBehaviour
{
    private int Gem = -1;
    private Text _textGem;

    private void Start()
    {
        _textGem = this.GetComponent<Text>();
    }

    private void Update()
    {
        if (RuntimeStorageData.IsReady)
        {
            if (RuntimeStorageData.Player.Gem != Gem)
            {
                Gem = RuntimeStorageData.Player.Gem;
                _textGem.text = Gem.ToString();
            }
        }
        else
        {
            Gem = 0;
            _textGem.text = Gem.ToString();
        }
    }
}
