using UnityEditor;
using UnityEngine;

public class InteractionCreator : EditorWindow
{
    private static string path = "Assets/First Person Controller Template/Resources/Prefabs/";

    [MenuItem("Tools/First Person Controller Template/Interaction Creator")]
    private static void Open() => GetWindow<InteractionCreator>();

    private void OnGUI()
    {
        if (GUILayout.Button("Create Door")) CreateDoor();
        if (GUILayout.Button("Create Key Item")) CreateKeyItem();
        if (GUILayout.Button("Create Gun Item")) CreateGunItem();
    }

    [MenuItem("GameObject/First Person Controller Template/Door")]
    public static void CreateDoor()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path + "TemplateDoor.prefab");
        PrefabUtility.InstantiatePrefab(prefab);
        Selection.activeObject = prefab;
    }

    [MenuItem("GameObject/First Person Controller Template/Key Item")]
    public static void CreateKeyItem()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path + "TemplateKeyItem.prefab");
        PrefabUtility.InstantiatePrefab(prefab);
        Selection.activeObject = prefab;
    }
    
    [MenuItem("GameObject/First Person Controller Template/Gun Item")]
    public static void CreateGunItem()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path + "TemplateGunItem.prefab");
        PrefabUtility.InstantiatePrefab(prefab);
        Selection.activeObject = prefab;
    }
}