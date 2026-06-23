using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "First Person Controller Template/Item Data")]
[Serializable]
public class ItemData : ScriptableObject
{
    public string itemName = null;
    public AudioClip dropSound = null;
    public float minDropForce = 2f;
    public float maxDropForce = 10f;
    [SerializeReference] public List<ItemComponent> components = new();

    public T GetComponent<T>() where T : ItemComponent
    {
        foreach (var component in components)
        {
            if (component is T result) return result;
        }
        return null;
    }

    public bool TryGetComponent<T>(out T result) where T : ItemComponent
    {
        foreach (var component in components)
        {
            if (component is T found)
            {
                result = found;
                return true;
            }
        }
        result = null;
        return false;
    }
}