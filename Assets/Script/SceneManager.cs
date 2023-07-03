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

    public SelectionControl _SelectionControl;
    public ChoppingControl _ChoppingControl;
    public SeasoningControl _SeasoningControl;

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
        }
    }

    public void Selection(string id)
    {
        _Id = id;
        NextStep();
    }

    public void WinGame()
    {

    }

    public void LoseGame()
    {

    }
}
