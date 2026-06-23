using UnityEngine;
using UnityEngine.EventSystems;

public class MobileLookArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private string horizontalAction = "LookX";
    [SerializeField] private string verticalAction = "LookY";
    [SerializeField] private float sensitivity = 0.15f;
    private int fingerId = -1;
    private Vector2 lastPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        fingerId = eventData.pointerId;
        lastPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != fingerId) return;
        fingerId = -1;
        InputManager.SetAxis(horizontalAction, 0);
        InputManager.SetAxis(verticalAction, 0);
    }

    private void Update()
    {
        InputManager.SetAxis(horizontalAction, 0);
        InputManager.SetAxis(verticalAction, 0);
        if (fingerId == -1) return;
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId != fingerId) continue;
            Vector2 delta = touch.position - lastPosition;
            lastPosition = touch.position;
            InputManager.SetAxis(horizontalAction, delta.x * sensitivity);
            InputManager.SetAxis(verticalAction, delta.y * sensitivity);
            return;
        }
    }
}