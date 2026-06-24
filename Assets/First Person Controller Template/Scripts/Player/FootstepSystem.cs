using UnityEngine;

public class FootstepSystem : MonoBehaviour
{
    [SerializeField] private FootstepSurface[] surfaces = null;
    [SerializeField] private float groundDistance = 2f;
    [SerializeField] private AudioSource audioSource = null;

    public void PlayFootstep()
    {
        if (!Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hit, groundDistance)) return;
        int hitLayer = hit.collider.gameObject.layer;
        foreach (var surface in surfaces)
        {
            if ((surface.layers.value & (1 << hitLayer)) == 0) continue;
            if (surface.clips.Length == 0) return;
            audioSource.PlayOneShot(surface.clips[Random.Range(0, surface.clips.Length)]);
            return;
        }
    }
}