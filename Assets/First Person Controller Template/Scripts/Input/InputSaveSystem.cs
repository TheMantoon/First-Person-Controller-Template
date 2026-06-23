using System.IO;
using UnityEngine;

public static class InputSaveSystem
{
    public static void Save(InputDatabase database, string fileName)
    {
        string json = JsonUtility.ToJson(database, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, fileName + ".json"), json);
    }

    public static void Load(InputDatabase database, string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");
        if (!File.Exists(path)) return;
        JsonUtility.FromJsonOverwrite(File.ReadAllText(path), database);
    }
}