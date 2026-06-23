using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InputActionData))]
public class InputActionDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var actionName = property.FindPropertyRelative("actionName");
        var inputType = property.FindPropertyRelative("inputType");
        var button = property.FindPropertyRelative("button");
        var axis = property.FindPropertyRelative("axis");
        var vector2 = property.FindPropertyRelative("vector2");
        float y = position.y;
        Rect rect = new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(rect, actionName);
        y += EditorGUIUtility.singleLineHeight + 2;
        rect.y = y;
        EditorGUI.PropertyField(rect, inputType);
        y += EditorGUIUtility.singleLineHeight + 2;
        InputType type = (InputType)inputType.enumValueIndex;
        rect.y = y;
        switch (type)
        {
            case InputType.Button:
                EditorGUI.PropertyField(rect, button, true);
                break;
            case InputType.Axis:
                EditorGUI.PropertyField(rect, axis, true);
                break;
            case InputType.Vector2:
                EditorGUI.PropertyField(rect, vector2, true);
                break;
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight * 2 + 4;
        var inputType = property.FindPropertyRelative("inputType");
        InputType type = (InputType)inputType.enumValueIndex;
        switch (type)
        {
            case InputType.Button:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("button"), true);
                break;
            case InputType.Axis:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("axis"), true);
                break;
            case InputType.Vector2:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("vector2"), true);
                break;
        }
        return height;
    }
}