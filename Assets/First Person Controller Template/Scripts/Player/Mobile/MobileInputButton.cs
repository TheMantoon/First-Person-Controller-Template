using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private string actionName = string.Empty;

    public void OnPointerDown(PointerEventData eventData) => InputManager.SetButtonDown(actionName);

    public void OnPointerUp(PointerEventData eventData) => InputManager.SetButtonUp(actionName);
}