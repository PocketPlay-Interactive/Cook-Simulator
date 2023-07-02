using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoSingleton<SceneManager>
{
    private IEnumerator Start()
    {
        //Loading every here
        CreateGame();

        yield return WaitForSecondCache.WAIT_TIME_ONE;
        Manager.Instance.HideGlobalLoading();
    }

    //task
    private int _TaskIndex = 0;

    public int GetTask() { return _TaskIndex; }

    //selection
    private string _Id = "";


    public void CreateGame()
    {
        _TaskIndex = 0;
        _Id = "";
        Step();
    }

    public void Step()
    {
        CameraControl.Instance.SetTask(_TaskIndex);
        StateControl.Instance.SetTask(_TaskIndex);
    }

    public void NextStep()
    {
        _TaskIndex += 1;
        CameraControl.Instance.SetTask(_TaskIndex);
        StateControl.Instance.SetTask(_TaskIndex);
    }

    public void Selection(string id)
    {
        _Id = id;
        NextStep();
    }
}
