using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public ItemInteractable takenItem = null;

    private void Awake() => Instance = this;

    private void Update()
    {
        if (InputManager.GetButtonDown("Drop") && !PlayerLook.Instance.paused && takenItem != null) Drop();
    }

    public void Drop()
    {
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        Rigidbody rigidbody = takenItem.GetComponent<Rigidbody>();
        Collider hitbox = takenItem.GetComponent<Collider>();
        rigidbody.isKinematic = false;
        hitbox.enabled = true;
        takenItem.transform.SetParent(null);
        Vector3 throwForce = (transform.forward + Vector3.up * 0.2f) * 3f + playerController.Velocity;
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);
        rigidbody.AddForce(throwForce, ForceMode.Impulse);
        foreach (var c in takenItem.GetComponentsInChildren<Transform>()) c.gameObject.layer = 6;
        takenItem.isTaken = false;
        takenItem = null;
    }
}