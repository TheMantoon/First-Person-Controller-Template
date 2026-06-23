using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private string actionName = "Move";
    [SerializeField] private RectTransform handle = null;
    [SerializeField] private float radius = 128f;
    private Vector2 input;

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rect = (RectTransform)transform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        Vector2 clamped = Vector2.ClampMagnitude(localPoint, radius);
        handle.anchoredPosition = clamped;
        input = clamped / radius;
        InputManager.SetVector2(actionName, input);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        InputManager.SetVector2(actionName, Vector2.zero);
    }
}