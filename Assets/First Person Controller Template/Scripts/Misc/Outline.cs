using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private float outlineScale = 1.05f;

    private Renderer[] renderers;

    private void Awake() => renderers = GetComponentsInChildren<Renderer>();

    public void SetVisible(bool visible)
    {
        foreach (var renderer in renderers) renderer.material.SetFloat("_OutlineScale", visible ? outlineScale : 1f);
    }
}