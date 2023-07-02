using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartRotation : MonoBehaviour
{
    public float speed = -45;
    private Vector3 _roation = Vector3.zero;

    private void Start()
    {
        _roation = _roation.WithZ(2 * speed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_roation * Time.deltaTime); // Xoay 45 độ theo trục y
    }
}
