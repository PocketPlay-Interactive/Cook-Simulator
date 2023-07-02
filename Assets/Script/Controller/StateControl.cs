using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateControl : MonoSingleton<StateControl>
{
    public Transform _Task;

    public void SetTask(int index)
    {
        _Task.ForChild((_indexTask, _childTask) =>
        {
            _childTask.ForChild((_child) => _child.SetActive(false));
            if (_indexTask < index)
                _childTask.FindChildByParent("Success").SetActive(true);
            else if(_indexTask == index)
                _childTask.FindChildByParent("Current").SetActive(true);
            else
                _childTask.FindChildByParent("Blank").SetActive(true);
        });
    }
}
