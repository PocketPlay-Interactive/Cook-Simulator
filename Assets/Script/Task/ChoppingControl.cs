using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingControl : MonoBehaviour
{
    public Transform _Knife;
    public float _Speed = 5.0f;  // tốc độ di chuyển của game object
    public float _LeftBound = -10.0f;    // giới hạn trái của game object
    public float _RightBound = 10.0f;    // giới hạn phải của game object

    private bool _MoveRight = true;      // biến xác định hướng di chuyển của game object


    private void Update()
    {
        if (SceneManager.Instance.GetTask() != 1)
            return;

        KnifeMoving();
    }

    private void KnifeMoving()
    {
        // Nếu game object di chuyển sang phải và đến giới hạn phải, đổi hướng di chuyển
        if (_MoveRight && _Knife.position.x > _RightBound)
            _MoveRight = false;

        // Nếu game object di chuyển sang trái và đến giới hạn trái, đổi hướng di chuyển
        if (!_MoveRight && _Knife.position.x < _LeftBound)
            _MoveRight = true;

        // Di chuyển game object sang phải hoặc trái tùy theo hướng di chuyển
        if (_MoveRight)
            _Knife.Translate(_Speed * Time.deltaTime, 0, 0);
        else
            _Knife.Translate(-_Speed * Time.deltaTime, 0, 0);

        Debug.Log(Mathf.Abs(_Knife.position.x - -0.0555f));
    }    
    //-0.0555
}
