using System;
using UnityEngine;

[Serializable]
public class InputActionData
{
    public string actionName = null;
    public InputType inputType = InputType.Button;
    public ButtonBinding button = null;
    public AxisBinding axis = null;
    public Vector2Binding vector2 = null;
}

public enum InputType
{
    Button,
    Axis,
    Vector2
}

[Serializable]
public class ButtonBinding
{
    public KeyCode primaryKey = KeyCode.None;
    public KeyCode secondaryKey = KeyCode.None;
}

[Serializable]
public class Vector2Binding
{
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
}

[Serializable]
public class AxisBinding
{
    public AxisSource source = AxisSource.MouseX;
    public float sensitivity = 1f;
}

public enum AxisSource
{
    MouseX,
    MouseY,
    ScrollWheel,
    Horizontal,
    Vertical
}