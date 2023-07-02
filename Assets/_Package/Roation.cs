using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roation : MonoBehaviour
{
    public float degreesPerSecond = 20;
    private Vector3 rotation = Vector3.zero;

    private void Start()
    {
        rotation = rotation.WithZ(-degreesPerSecond);
    }

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }
}
