using UnityEngine;
using UnityEngine.EventSystems;

public enum TouchType
{
    Down,
    Up,
    Enter,
    Drag
}

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IDragHandler
{
  
    public void OnPointerDown(PointerEventData eventData)
    {
        SceneManager.Instance.TouchCall(TouchType.Down, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SceneManager.Instance.TouchCall(TouchType.Up, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SceneManager.Instance.TouchCall(TouchType.Enter, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SceneManager.Instance.TouchCall(TouchType.Drag, eventData);
    }
}