using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ServingControl : MonoBehaviour
{
    public Transform[] _Models;
    private List<Vector3> _LstModelLocalDefaultPosition = new List<Vector3>();

    public Transform _Character;
    public Transform _TargetPosition;

    public void AwakeCall()
    {
        _Models.ForEach((_Model, _Index) =>
        {
            _Model.localPosition = _LstModelLocalDefaultPosition[_Index];
        });
    }
    // Start is called before the first frame update
    private void Start()
    {
        _Models.ForEach((_Model) =>
        {
            _LstModelLocalDefaultPosition.Add(_Model.localPosition);
        });
    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneManager.Instance.GetTask() != 4)
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
        if(hit.transform.name == "RibEye_Sliced")
        {
            _Models.ForEach((_Model, _Index) =>
            {
                if (!_Model.IsActive())
                    return;

                _Model.DOKill();
                _Model.DOMove(_TargetPosition.position, 0.5f);
                _Model.DOScale(Vector3.zero, 0.7f).OnComplete(() =>
                {
                    _Character.GetComponent<Animator>().Play("success");
                    CoroutineUtils.PlayCoroutine(() =>
                    {
                        SceneManager.Instance.WinGame();
                    }, 1f);
                });
            });
        }
    }

    public void TouchCall(TouchType touch, PointerEventData eventData)
    {

    }
}
