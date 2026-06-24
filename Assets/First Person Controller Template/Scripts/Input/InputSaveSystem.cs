using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class InputSaveData
{
    public List<ButtonBindingData> buttons = new();
    public List<AxisBindingData> axes = new();
    public List<Vector2BindingData> vectors = new();
}

[Serializable]
public class ButtonBindingData
{
    public string actionName;
    public KeyCode primaryKey;
    public KeyCode secondaryKey;
}

[Serializable]
public class AxisBindingData
{
    public string actionName;
    public AxisSource source;
    public float sensitivity;
}

[Serializable]
public class Vector2BindingData
{
    public string actionName;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
}

public static class InputSaveSystem
{
    public static void Save(InputDatabase database, string fileName)
    {
        InputSaveData saveData = new();
        foreach (var action in database.actions)
        {
            switch (action.inputType)
            {
                case InputType.Button:
                    saveData.buttons.Add(new ButtonBindingData
                    {
                        actionName = action.actionName,
                        primaryKey = action.button.primaryKey,
                        secondaryKey = action.button.secondaryKey
                    });
                    break;
                case InputType.Axis:
                    saveData.axes.Add(new AxisBindingData
                    {
                        actionName = action.actionName,
                        source = action.axis.source,
                        sensitivity = action.axis.sensitivity
                    });
                    break;
                case InputType.Vector2:
                    saveData.vectors.Add(new Vector2BindingData
                    {
                        actionName = action.actionName,
                        up = action.vector2.up,
                        down = action.vector2.down,
                        left = action.vector2.left,
                        right = action.vector2.right
                    });
                    break;
            }
        }
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, fileName + ".json"), json);
    }

    public static void Load(InputDatabase database, string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        if (!File.Exists(path)) return;
        InputSaveData saveData = JsonUtility.FromJson<InputSaveData>(File.ReadAllText(path));
        foreach (var action in database.actions)
        {
            if (action.inputType == InputType.Button)
            {
                var saved = saveData.buttons.Find(x => x.actionName == action.actionName);
                if (saved != null)
                {
                    action.button.primaryKey = saved.primaryKey;
                    action.button.secondaryKey = saved.secondaryKey;
                }
            }
            if (action.inputType == InputType.Axis)
            {
                var saved = saveData.axes.Find(x => x.actionName == action.actionName);
                if (saved != null)
                {
                    action.axis.source = saved.source;
                    action.axis.sensitivity = saved.sensitivity;
                }
            }
            if (action.inputType == InputType.Vector2)
            {
                var saved = saveData.vectors.Find(x => x.actionName == action.actionName);
                if (saved != null)
                {
                    action.vector2.up = saved.up;
                    action.vector2.down = saved.down;
                    action.vector2.left = saved.left;
                    action.vector2.right = saved.right;
                }
            }
        }
    }
}