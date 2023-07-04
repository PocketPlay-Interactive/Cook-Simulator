using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoppingControl : MonoBehaviour
{
    public Transform _Knife;
    public Transform _KnifeObject;

    public Transform[] _Models;

    private Vector3 _KnifeDefaultLocalPosition;

    private float _Speed = 0.03f;  // tốc độ di chuyển của game object
    private float _LeftBound = -0.0555f;    // giới hạn trái của game object
    private float _RightBound = 0.0f;    // giới hạn phải của game object
    private bool _MoveRight = true;      // biến xác định hướng di chuyển của game object
    private bool _Tap = false;

    public LineRenderer _CutLine;

    //private List<Vector3> _ChildModelDefaultPositions = new List<Vector3>();

    private void Start()
    {
        _KnifeDefaultLocalPosition = _KnifeObject.localPosition;
    }

    public void AwakeCall()
    {
        _MoveRight = false;
        _Tap = false;

        _KnifeObject.localPosition = _KnifeDefaultLocalPosition;
        _Models.SimpleForEach((_Child) =>
        {
            if (!_Child.IsActive())
                return;
            var _Script = _Child.GetComponent<ChoppingModelControl>();
            _Script.AwakeCall();
        });
    }

    private void Update()
    {
        if (SceneManager.Instance.GetTask() != 1)
            return;

        KnifeMoving();
    }

    private float min = 1;
    private float center = 0.5f;
    private float max = 0;

    private void KnifeMoving()
    {
        if (_Tap)
            return;

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

        var distance = Mathf.Abs(_Knife.position.x - -0.0555f);
        if (distance < min)
            min = distance;
        if (distance > max)
            max = distance;

        center = (max + min) / 2;
        ChangeColor(distance);
    }

    Color startColor = Color.green;
    Color endColor = Color.red;
    private void ChangeColor(float value)
    {
        float t = 0;

        if (value <= center)
        {
            startColor = Color.green;
            endColor = Color.yellow;
            t = Mathf.Clamp01(value / center);
        }
        else if (value <= max)
        {
            startColor = Color.yellow;
            endColor = Color.red;
            t = Mathf.Clamp01((value - center) / center);
        }

        //LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Material material = _CutLine.material;
        material.SetColor("_Color", Color.Lerp(startColor, endColor, t));
    }

    public void TouchCall(TouchType touch)
    {
        if(touch == TouchType.Down)
        {
            _Tap = true;
            var caculateSpeedY = -_Speed / 10.0f;
#if UNITY_EDITOR
            caculateSpeedY = -_Speed / 3.0f;
#endif
            if (_KnifeObject.localPosition.y <= -0.03791597)
            {
                var distance = Mathf.Abs(_Knife.position.x - -0.0555f);
                if(distance >= center * 0.65f)
                    SceneManager.Instance.LoseGame();
                else
                {
                    _Models.SimpleForEach((_Child) =>
                    {
                        if (!_Child.IsActive())
                            return;
                        var _Script = _Child.GetComponent<ChoppingModelControl>();
                        _Script.GetAllSlice().SimpleForEach((_Instanties) =>
                        {
                            _Instanties.parent = _KnifeObject;
                            _Instanties.SetActive(true);
                        });
                    });

                    CoroutineUtils.PlayCoroutine(() =>
                    {
                        _KnifeObject.DOKill();
                        _KnifeObject.DOLocalMoveX(0.5f, 0.5f).OnComplete(() =>
                        {
                            CoroutineUtils.PlayCoroutine(() => SceneManager.Instance.NextStep(), 0.2f);
                        });
                    }, 0.1f);
                }
            }
            else
            {
                _KnifeObject.Translate(0, caculateSpeedY, 0);
            }
        }    
    }
}
