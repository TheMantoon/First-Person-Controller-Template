using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    private SerializedProperty itemName;
    private SerializedProperty dropSound;
    private SerializedProperty minDropForce;
    private SerializedProperty maxDropForce;
    private SerializedProperty components;

    private void OnEnable()
    {
        itemName = serializedObject.FindProperty("itemName");
        dropSound = serializedObject.FindProperty("dropSound");
        minDropForce = serializedObject.FindProperty("minDropForce");
        maxDropForce = serializedObject.FindProperty("maxDropForce");
        components = serializedObject.FindProperty("components");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawItemData();
        EditorGUILayout.Space();
        DrawDropSound();
        EditorGUILayout.Space();
        DrawComponents();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawItemData()
    {
        EditorGUILayout.LabelField("Item Data", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(itemName);
    }

    private void DrawDropSound()
    {
        EditorGUILayout.LabelField("Drop Sound", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(dropSound);
        EditorGUILayout.PropertyField(minDropForce);
        EditorGUILayout.PropertyField(maxDropForce);
    }

    private void DrawComponents()
    {
        EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);
        for (int i = 0; i < components.arraySize; i++)
        {
            SerializedProperty element = components.GetArrayElementAtIndex(i);
            string typeName = "Null";
            if (element.managedReferenceValue != null) typeName = element.managedReferenceValue.GetType().Name;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(typeName, EditorStyles.boldLabel);
            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                components.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(element, true);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Add Component")) ShowAddComponentMenu();
    }

    private void ShowAddComponentMenu()
    {
        GenericMenu menu = new GenericMenu();
        foreach (Type type in TypeCache.GetTypesDerivedFrom<ItemComponent>())
        {
            if (type.IsAbstract) continue;
            menu.AddItem(
                new GUIContent(type.Name),
                false,
                () =>
                {
                    serializedObject.Update();
                    int index = components.arraySize;
                    components.InsertArrayElementAtIndex(index);
                    SerializedProperty element = components.GetArrayElementAtIndex(index);
                    element.managedReferenceValue = Activator.CreateInstance(type);
                    serializedObject.ApplyModifiedProperties();
                });
        }
        menu.ShowAsContext();
    }
}