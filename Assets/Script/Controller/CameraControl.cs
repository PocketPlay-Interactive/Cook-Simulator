using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoSingleton<CameraControl>
{
    public Transform _Camera;
    public Transform[] _LstCameraPosition;

    private int _PositionCamera = 0;

    public void SetTask(int taskIndex)
    {
        _PositionCamera = taskIndex;
        if (taskIndex == 0)
        {
            _Camera.transform.position = _LstCameraPosition[_PositionCamera].position;
            _Camera.transform.rotation = _LstCameraPosition[_PositionCamera].rotation;
        }
        else
        {
            _Camera.DOKill();
            _Camera.DOMove(_LstCameraPosition[_PositionCamera].position, 0.5f);
            _Camera.DORotateQuaternion(_LstCameraPosition[_PositionCamera].rotation, 0.5f);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _PositionCamera += 1;
            if (_PositionCamera >= _LstCameraPosition.Length)
                _PositionCamera = 0;

            _Camera.DOKill();
            _Camera.DOMove(_LstCameraPosition[_PositionCamera].position, 0.5f);
            _Camera.DORotateQuaternion(_LstCameraPosition[_PositionCamera].rotation, 0.5f);
        }
    }
}
