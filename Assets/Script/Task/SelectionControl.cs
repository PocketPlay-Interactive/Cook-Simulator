using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionControl : MonoBehaviour
{
    public void AwakeCall()
    {

    }    

    private void Update()
    {
        if (SceneManager.Instance.GetTask() != 0)
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
        if (hit.transform.name != "RibEye" &&
            hit.transform.name != "Sirloin" &&
            hit.transform.name != "T-Bone")
            return;

        SceneManager.Instance.Selection(hit.transform.name);
    }

    public void TouchCall(TouchType touch)
    {

    }
}
