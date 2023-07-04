using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneManager : MonoSingleton<SceneManager>
{
    private IEnumerator Start()
    {
        //Loading every here
        CreateGame();

        yield return WaitForSecondCache.WAIT_TIME_ONE;
        Manager.Instance.HideGlobalLoading();
    }

    //public Transform _LoseUI;
    //public Transform _WinUI;

    [Header("Control")]

    public SelectionControl _SelectionControl;
    public ChoppingControl _ChoppingControl;
    public SeasoningControl _SeasoningControl;
    public CookingControl _CookingControl;
    public ServingControl _ServingControl;

    //task
    private int _TaskIndex = -1;

    public int GetTask() { return _TaskIndex; }

    //selection
    private string _Id = "";


    public void CreateGame()
    {
        _TaskIndex = -1;
        _Id = "";
        NextStep();
    }

    public void NextStep()
    {
        _TaskIndex += 1;
        CameraControl.Instance.SetTask(_TaskIndex);
        StateControl.Instance.SetTask(_TaskIndex);
        ScenePanel.Instance.SetText(_TaskIndex);

        switch(_TaskIndex)
        {
            case 0:
                _SelectionControl.AwakeCall();
                break;
            case 1:
                _ChoppingControl.AwakeCall();
                break;
            case 2:
                _SeasoningControl.AwakeCall();
                break;
            case 3:
                _CookingControl.AwakeCall();
                break;
            case 4:
                _ServingControl.AwakeCall();
                break;
        }    
    }

    public void TouchCall(TouchType touch, PointerEventData eventData)
    {
        switch (_TaskIndex)
        {
            case 0:
                _SelectionControl.TouchCall(touch);
                break;
            case 1:
                _ChoppingControl.TouchCall(touch);
                break;
            case 2:
                _SeasoningControl.TouchCall(touch, eventData);
                break;
            case 3:
                _CookingControl.TouchCall(touch, eventData);
                break;
            case 4:
                _CookingControl.TouchCall(touch, eventData);
                break;
        }
    }

    public void Selection(string id)
    {
        _Id = id;
        NextStep();
    }

    public void WinGame()
    {
        ScenePanel.Instance._SuccessPanel.Show();
    }

    public void LoseGame()
    {
        ScenePanel.Instance._FailurePanel.Show(_TaskIndex);
    }
}
