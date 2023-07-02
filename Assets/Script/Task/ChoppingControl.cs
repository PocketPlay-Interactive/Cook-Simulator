using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingControl : MonoBehaviour
{
    public Transform _Knife;

    private void Update()
    {
        if (SceneManager.Instance.GetTask() != 1)
            return;

        //KnifeMoving();
    }

    private void KnifeMoving()
    {

    }    
    //-0.0555
}
