using UnityEngine;

[CreateAssetMenu(menuName = "First Person Controller Template/Footstep Surface")]
public class FootstepSurface : ScriptableObject
{
    public LayerMask layers = ~0;
    public AudioClip[] clips = null;
}