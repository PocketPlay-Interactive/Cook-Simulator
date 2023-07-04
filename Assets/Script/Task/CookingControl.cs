using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CookingControl : MonoBehaviour
{
    private string _IdSelection = "";

    public Transform[] _Models;
    public Transform[] _Fires;
    public Transform _Knob;
    public float _Sensitivity = 1f;
    public float _FireStrong = 0.5f;
    public float _ChangeColorSpeed = 1;

    private Material _FadeMaterial;
    private float ThermalOverloadTimer = 0;

    public void AwakeCall()
    {
        _Fires.SimpleForEach((_Fire) =>
        {
            _Fire.localScale = _Fire.localScale.WithY(0.1f * _FireStrong);
        });

        _Models.SimpleForEach((_Model) =>
        {
            if (!_Model.IsActive())
                return;

            _FadeMaterial = _Model.FindChildByParent("Full1").GetComponent<SkinnedMeshRenderer>().materials[0];
            _FadeMaterial.SetColor("_Color", _FadeMaterial.GetColor("_Color").WithAlpha(0));
        });

        ThermalOverloadTimer = 0;
    }
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneManager.Instance.GetTask() != 3)
            return;

        if (Input.GetMouseButtonDown(0)) // Kiểm tra nút chuột trái được nhấn
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) // Kiểm tra xem raycast có va chạm với đối tượng nào không
            {
                CheckingRaycast(hit);
            }
        }

        UpdateColor();
    }

    private void UpdateColor()
    {
        var alphaColor = _FadeMaterial.GetColor("_Color").GetAlpha();
        alphaColor += _ChangeColorSpeed * _FireStrong * Time.deltaTime;
        alphaColor = Mathf.Clamp01(alphaColor);
        _FadeMaterial.SetColor("_Color", _FadeMaterial.GetColor("_Color").WithAlpha(alphaColor));

        if(alphaColor > 0.98f)
        {
            if(_FireStrong < 0.2f)
            {
                SceneManager.Instance.NextStep();
            }

            ThermalOverloadTimer += Time.deltaTime;
            if (ThermalOverloadTimer > 2f)
            {
                SceneManager.Instance.LoseGame();
            }
        }
    }

    private void CheckingRaycast(RaycastHit hit)
    {
        _IdSelection = hit.transform.name;
    }

    public void TouchCall(TouchType touch, PointerEventData eventData)
    {
        if (touch == TouchType.Down)
        {

        }

        if (touch == TouchType.Drag)
        {
            if(_IdSelection == _Knob.name)
            {
                Vector2 dragDelta = eventData.delta;
                Vector3 moveDelta = VectorExtensions.Create(dragDelta.x) * _Sensitivity;
                _Knob.Translate(moveDelta, Space.World);

                if (_Knob.localPosition.x > 0.17f)
                    _Knob.localPosition = _Knob.localPosition.WithX(0.17f);
                else if(_Knob.localPosition.x < -0.17f)
                    _Knob.localPosition = _Knob.localPosition.WithX(-0.17f);

                _FireStrong = Mathf.Clamp01((_Knob.localPosition.x + 0.17f) / 0.34f);

                _Fires.SimpleForEach((_Fire) =>
                {
                    _Fire.localScale = _Fire.localScale.WithY(0.11f * _FireStrong);
                });
            }
        }

        if (touch == TouchType.Up)
        {

        }
    }
}
