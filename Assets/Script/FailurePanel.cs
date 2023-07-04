using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailurePanel : MonoBehaviour
{
    public Text _Description;
    public void Show(int _TaskIndex)
    {
        switch(_TaskIndex)
        {
            case 0:
                _Description.text = "You need to choose the right piece of meat.";
                break;
            case 1:
                _Description.text = "You didn't cut the blue line.";
                break;
            case 3:
                _Description.text = "You forgot to turn off the stove.";
                break;
        }

        this.gameObject.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.Instance.CreateGame();
        this.gameObject.SetActive(false);
    }
}
