using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingModelControl : MonoBehaviour
{
    public List<Transform> _LstChild = new List<Transform>();
    public List<Vector3> _LstChildDefaultPosition = new List<Vector3>();
    private void Start()
    {
        transform.ForChild((_Child) =>
        {
            if (!_Child.name.StartsWith("Slice"))
                return;
            _LstChild.Add(_Child);
            _LstChildDefaultPosition.Add(_Child.localPosition);
        });
    }

    public void AwakeCall()
    {
        _LstChild.SimpleForEach((_Child, _Index) =>
        {
            _Child.parent = transform;
            _Child.localPosition = _LstChildDefaultPosition[_Index];
        });
    }

    public Transform[] GetAllSlice()
    {
        return _LstChild.ToArray();
    }
}
