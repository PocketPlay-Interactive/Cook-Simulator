using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessPanel : MonoBehaviour
{
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.Instance.CreateGame();
        this.gameObject.SetActive(false);
    }
}
