using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance = null;
    public InputDatabase database = null;
    private Dictionary<string, InputActionData> actionMap = null;
    private static Dictionary<string, bool> virtualButtons = new();
    private static Dictionary<string, bool> virtualButtonsDown = new();
    private static Dictionary<string, bool> virtualButtonsUp = new();
    private static Dictionary<string, float> virtualAxes = new();
    private static Dictionary<string, Vector2> virtualVector2s = new();

    private void Awake()
    {
        Instance = this;
        actionMap = new Dictionary<string, InputActionData>();
        foreach (var action in database.actions) actionMap[action.actionName] = action;
    }

    private void LateUpdate()
    {
        virtualButtonsDown.Clear();
        virtualButtonsUp.Clear();
    }

    public static void SetButton(string action, bool value) => virtualButtons[action] = value;

    public static void SetButtonDown(string action)
    {
        virtualButtonsDown[action] = true;
        virtualButtons[action] = true;
    }

    public static void SetButtonUp(string action)
    {
        virtualButtonsUp[action] = true;
        virtualButtons[action] = false;
    }

    public static void SetAxis(string action, float value) => virtualAxes[action] = value;

    public static void SetVector2(string action, Vector2 value) => virtualVector2s[action] = value;

    public static bool GetButton(string action, bool safe = true)
    {
        bool virtualValue = virtualButtons.TryGetValue(action, out bool pressed) && pressed;
        if (!TryGetAction(action, InputType.Button, out var data)) return virtualValue;
        bool primary = safe ? GetKeySafe(data.button.primaryKey) : Input.GetKey(data.button.primaryKey);
        bool secondary = safe ? GetKeySafe(data.button.secondaryKey) : Input.GetKey(data.button.secondaryKey);
        return virtualValue || primary || secondary;
    }

    public static bool GetButtonDown(string action, bool safe = true)
    {
        bool virtualDown = virtualButtonsDown.TryGetValue(action, out bool pressed) && pressed;
        if (!TryGetAction(action, InputType.Button, out var data)) return virtualDown;
        bool primary = safe ? GetKeyDownSafe(data.button.primaryKey) : Input.GetKeyDown(data.button.primaryKey);
        bool secondary = safe ? GetKeyDownSafe(data.button.secondaryKey) : Input.GetKeyDown(data.button.secondaryKey);
        return virtualDown || primary || secondary;
    }

    public static bool GetButtonUp(string action, bool safe = true)
    {
        bool virtualUp = virtualButtonsUp.TryGetValue(action, out bool pressed) && pressed;
        if (!TryGetAction(action, InputType.Button, out var data)) return virtualUp;
        bool primary = safe ? GetKeyUpSafe(data.button.primaryKey) : Input.GetKeyUp(data.button.primaryKey);
        bool secondary = safe ? GetKeyUpSafe(data.button.secondaryKey) : Input.GetKeyUp(data.button.secondaryKey);
        return virtualUp || primary || secondary;
    }

    public static float GetAxis(string action)
    {
        bool hasVirtual = virtualAxes.TryGetValue(action, out float virtualValue);
        if (!TryGetAction(action, InputType.Axis, out var data)) return hasVirtual ? virtualValue : 0f;
        float inputValue = 0f;
        switch (data.axis.source)
        {
            case AxisSource.MouseX:
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                inputValue = 0;
#else
                inputValue = Input.GetAxisRaw("Mouse X") * data.axis.sensitivity;
#endif
                break;
            case AxisSource.MouseY:
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                inputValue = 0;
#else
                inputValue = Input.GetAxisRaw("Mouse Y") * data.axis.sensitivity;
#endif
                break;
            case AxisSource.ScrollWheel:
                inputValue = Input.GetAxisRaw("Mouse ScrollWheel") * data.axis.sensitivity;
                break;
            case AxisSource.Horizontal:
                inputValue = Input.GetAxisRaw("Horizontal") * data.axis.sensitivity;
                break;
            case AxisSource.Vertical:
                inputValue = Input.GetAxisRaw("Vertical") * data.axis.sensitivity;
                break;
        }
        if (hasVirtual && Mathf.Abs(virtualValue) > 0.001f) return virtualValue;
        return inputValue;
    }

    public static Vector2 GetVector2(string action)
    {
        bool hasVirtual = virtualVector2s.TryGetValue(action, out Vector2 virtualValue);
        if (!TryGetAction(action, InputType.Vector2, out var data)) return hasVirtual ? virtualValue : Vector2.zero;
        Vector2 keyboardValue = Vector2.zero;
        if (Input.GetKey(data.vector2.up)) keyboardValue.y += 1;
        if (Input.GetKey(data.vector2.down)) keyboardValue.y -= 1;
        if (Input.GetKey(data.vector2.right)) keyboardValue.x += 1;
        if (Input.GetKey(data.vector2.left)) keyboardValue.x -= 1;
        keyboardValue.Normalize();
        if (hasVirtual && virtualValue.sqrMagnitude > 0.001f) return virtualValue;
        return keyboardValue;
    }

    private static bool GetKeySafe(KeyCode key)
{
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    if (IsMouseKey(key))
        return false;
#endif

    return Input.GetKey(key);
}

    private static bool GetKeyDownSafe(KeyCode key)
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (IsMouseKey(key)) return false;
#endif
        return Input.GetKeyDown(key);
    }

    private static bool GetKeyUpSafe(KeyCode key)
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (IsMouseKey(key)) return false;
#endif
        return Input.GetKeyUp(key);
    }

    private static bool TryGetAction(string action, InputType expectedType, out InputActionData data)
    {
        data = null;
        if (Instance == null) return false;
        if (!Instance.actionMap.TryGetValue(action, out data)) return false;
        return data.inputType == expectedType;
    }

    private static bool IsMouseKey(KeyCode key)
    {
        return key == KeyCode.Mouse0 || key == KeyCode.Mouse1 || key == KeyCode.Mouse2 ||
               key == KeyCode.Mouse3 || key == KeyCode.Mouse4 || key == KeyCode.Mouse5 || key == KeyCode.Mouse6;
    }
}