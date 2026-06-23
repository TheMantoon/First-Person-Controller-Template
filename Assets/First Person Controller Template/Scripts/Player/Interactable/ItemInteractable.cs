using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemInteractable : MonoBehaviour, IInteractable
{
    public ItemData itemData = null;
    public bool isTaken = false;
    private float soundCooldown = 0.4f;
    private float lastSoundTime = 0;
    private Collider hitbox = null;
    private Rigidbody rb = null;
    private AudioSource audioSource = null;

    private void Awake()
    {
        hitbox = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (isTaken) return;
        if (ItemManager.Instance?.takenItem != null) ItemManager.Instance.Drop();
        hitbox.enabled = false;
        rb.isKinematic = true;
        transform.SetParent(ItemManager.Instance.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        ItemManager.Instance.takenItem = this;
        isTaken = true;
        foreach (var c in gameObject.GetComponentsInChildren<Transform>()) c.gameObject.layer = 3;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTaken) return;
        if (Time.time - lastSoundTime < soundCooldown) return;
        lastSoundTime = Time.time;
        float impactForce = collision.relativeVelocity.magnitude;
        if (impactForce < itemData.minDropForce) return;
        float volume = Mathf.InverseLerp(itemData.minDropForce, itemData.maxDropForce, impactForce);
        if (itemData.dropSound != null) audioSource.PlayOneShot(itemData.dropSound, volume);
    }

    public void PreviewEnter() => GetComponent<Outline>().SetVisible(true);

    public void PreviewExit() => GetComponent<Outline>().SetVisible(false);

    public string GetName()
    {
        return itemData.itemName;
    }
}