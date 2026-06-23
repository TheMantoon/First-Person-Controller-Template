using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "First Person Controller Template/Input Database")]
public class InputDatabase : ScriptableObject
{
    public List<InputActionData> actions = new();
}