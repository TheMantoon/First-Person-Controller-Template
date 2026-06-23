using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string doorName = "Door";
    [SerializeField] private float openAngle = -90f, speed = 5f;
    [SerializeField] private bool locked = false;
    [SerializeField] private Transform pivot = null;
    [SerializeField] private AudioClip openClip = null, closeClip = null, lockedClip = null, unlockClip = null;
    [SerializeField] private string needKeyID = string.Empty;
    private AudioSource audioSource = null;
    private bool isOpen = false;
    private Quaternion closedRot = new Quaternion();
    private Quaternion openRot = new Quaternion();
    private Rigidbody rb;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = pivot.GetComponent<Rigidbody>();
        closedRot = pivot.localRotation;
        openRot = Quaternion.Euler(0, openAngle, 0) * closedRot;
    }

    public void Interact()
    {
        if (locked)
        {
            var item = ItemManager.Instance?.takenItem?.itemData;
            if (item != null && item.TryGetComponent<KeyComponent>(out var key))
            {
                if (needKeyID == key.keyID)
                {
                    locked = false;
                    audioSource.PlayOneShot(unlockClip);
                    return;
                }
            }
            if (!isOpen)
            {
                audioSource.PlayOneShot(lockedClip);
                return;
            }
        }
        isOpen = !isOpen;
        if (isOpen) audioSource.PlayOneShot(openClip);
        else audioSource.PlayOneShot(closeClip);
    }

    private void FixedUpdate()
    {
        Quaternion target = isOpen ? openRot : closedRot;
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, speed * Time.fixedDeltaTime));
    }

    public void PreviewEnter() => GetComponent<Outline>().SetVisible(true);

    public void PreviewExit() => GetComponent<Outline>().SetVisible(false);

    public string GetName()
    {
        return doorName;
    }
}