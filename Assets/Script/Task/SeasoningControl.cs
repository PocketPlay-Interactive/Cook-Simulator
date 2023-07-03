using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeasoningControl : MonoBehaviour
{
    private Vector2 _DragStartPosition;

    public Transform[] _Models;
    public Transform[] _ShakerModels;
    public Transform _TongsModels;
    private string _IdSelection = "";
    private List<Vector3> _LstShakerDefaultPosition = new List<Vector3>();
    private List<Quaternion> _LstShakerDefaultRotation = new List<Quaternion>();
    private Vector3 TongsDefaultPosition;

    public float _Sensitivity = 1f;

    private void Start()
    {
        _ShakerModels.SimpleForEach((_Shaker) =>
        {
            _LstShakerDefaultPosition.Add(_Shaker.localPosition);
            _LstShakerDefaultRotation.Add(_Shaker.localRotation);
        });
        TongsDefaultPosition = _TongsModels.localPosition;
    }

    public void AwakeCall()
    {

    }

    private void Update()
    {
        if (SceneManager.Instance.GetTask() != 2)
            return;

        if (Input.GetMouseButtonDown(0)) // Kiểm tra nút chuột trái được nhấn
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) // Kiểm tra xem raycast có va chạm với đối tượng nào không
            {
                CheckingRaycast(hit);
            }
        }
    }

    private void CheckingRaycast(RaycastHit hit)
    {
        Debug.Log(hit.transform.name);
        if (hit.transform.name != "Tongs" &&
            hit.transform.name != "SaltShaker" &&
            hit.transform.name != "OreganoShaker" &&
            hit.transform.name != "PaprikaShaker")
        {
            _IdSelection = "";
            _ShakerModels.SimpleForEach((_Shaker, _Index) =>
            {
                _Shaker.DOKill();
                _Shaker.DOLocalMove(_LstShakerDefaultPosition[_Index], 0.3f);
                _Shaker.DOLocalRotateQuaternion(_LstShakerDefaultRotation[_Index], 0.3f);
            });

            _TongsModels.DOKill();
            _TongsModels.DOLocalMove(TongsDefaultPosition, 0.3f);
        }
        else
        {
            _IdSelection = hit.transform.name;
            switch(_IdSelection)
            {
                case "Tongs":
                    _ShakerModels.SimpleForEach((_Shaker, _Index) =>
                    {
                        _Shaker.DOKill();
                        _Shaker.DOLocalMove(_LstShakerDefaultPosition[_Index], 0.3f);
                        _Shaker.DOLocalRotateQuaternion(_LstShakerDefaultRotation[_Index], 0.3f);
                    });
                    _TongsModels.DOKill();
                    _TongsModels.DOLocalMove(VectorExtensions.Create(-0.025f, 0, 0), 0.3f);
                    break;
                default:
                    _ShakerModels.SimpleForEach((_Shaker, _Index) =>
                    {
                        if (_Shaker.name == hit.transform.name)
                        {
                            _Shaker.DOKill();
                            _Shaker.DOLocalMove(VectorExtensions.Create(0.08f, 0, -0.08f), 0.3f);
                            _Shaker.DOLocalRotateQuaternion(Quaternion.Euler(180f, 0f, 0f), 0.3f);
                        }
                        else
                        {
                            _Shaker.DOKill();
                            _Shaker.DOLocalMove(_LstShakerDefaultPosition[_Index], 0.3f);
                            _Shaker.DOLocalRotateQuaternion(_LstShakerDefaultRotation[_Index], 0.3f);
                        }
                    });
                    _TongsModels.DOKill();
                    _TongsModels.DOLocalMove(TongsDefaultPosition, 0.3f);
                    break;
            }
        }
    }

    public void TouchCall(TouchType touch, PointerEventData eventData)
    {
        if(touch == TouchType.Down)
        {
            if (_IdSelection == "Tongs")
            {
                if (eventData.button != PointerEventData.InputButton.Left)
                    return;

                _DragStartPosition = eventData.position;
            }
        }

        if(touch == TouchType.Drag)
        {
            if(_IdSelection == "Tongs")
            {
                if (eventData.button != PointerEventData.InputButton.Left)
                    return;
                Vector2 dragCurrentPosition = eventData.position;
                Vector2 difference = dragCurrentPosition - _DragStartPosition;
                float rotationZ = -difference.x / 3f;
                rotationZ = -Mathf.Clamp(rotationZ, 0f, 180f);
                Quaternion newRotation = Quaternion.Euler(180f, 0f, rotationZ);
                _Models.SimpleForEach((_child) =>
                {
                    if (!_child.IsActive())
                        return;
                    _child.rotation = newRotation;
                });
            }
            else
            {
                _ShakerModels.SimpleForEach((_Shaker) =>
                {
                    if (_Shaker.name != _IdSelection)
                        return;
                    // Lấy khoảng cách di chuyển của ngón tay trên màn hình
                    Vector2 dragDelta = eventData.delta;

                    // Chuyển đổi khoảng cách di chuyển thành khoảng cách trong thế giới 3D
                    Vector3 moveDelta = new Vector3(dragDelta.x, 0f, dragDelta.y) * _Sensitivity;

                    // Di chuyển đối tượng theo khoảng cách mới
                    _Shaker.Translate(moveDelta, Space.World);
                    var Salt = _Shaker.FindChildByParent("Salt");
                    Salt.SetActive(true);
                    var LstParticleSystem = Salt.GetComponentsInChildren<ParticleSystem>();
                    LstParticleSystem.SimpleForEach((_Effect) =>
                    {
                        _Effect.Play();
                    });
                });
            }
        }

        if (touch == TouchType.Up)
        {
            if (_IdSelection == "Tongs")
            {
                //_IdSelection = "";
                //_TongsModels.DOKill();
                //_TongsModels.DOLocalMove(TongsDefaultPosition, 0.3f);
            }
            else if (_IdSelection == "SaltShaker" ||
                _IdSelection == "OreganoShaker" ||
                _IdSelection == "PaprikaShaker")
            {
                _ShakerModels.SimpleForEach((_Shaker) =>
                {
                    if (_Shaker.name != _IdSelection)
                        return;
                    var Salt = _Shaker.FindChildByParent("Salt");
                    Salt.SetActive(true);
                    var LstParticleSystem = Salt.GetComponentsInChildren<ParticleSystem>();
                    LstParticleSystem.SimpleForEach((_Effect) =>
                    {
                        _Effect.Stop();
                    });
                });
            }
        }
    }
}
