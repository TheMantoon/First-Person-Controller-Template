using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;
    [SerializeField] private Camera playerCamera = null;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactableMask = ~0;
    [SerializeField] private Text itemName = null;
    public Text ammoText = null;
    [SerializeField] private RawImage crosshair = null;
    [SerializeField] private Texture2D crosshairSprite = null, handSprite = null;
    private IInteractable currentInteractable;

    private void Awake() => Instance = this;

    private void Update()
    {
        IInteractable newInteractable = null;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit,
            distance, interactableMask))
            hit.collider.TryGetComponent(out newInteractable);
        if (currentInteractable != newInteractable)
        {
            currentInteractable?.PreviewExit();
            newInteractable?.PreviewEnter();
            currentInteractable = newInteractable;
            if (currentInteractable != null)
            {
                itemName.gameObject.SetActive(true);
                itemName.text = currentInteractable.GetName();
                crosshair.texture = handSprite;
                crosshair.rectTransform.sizeDelta = new Vector2(48, 48);
            }
            else
            {
                itemName.gameObject.SetActive(false);
                itemName.text = string.Empty;
                crosshair.texture = crosshairSprite;
                crosshair.rectTransform.sizeDelta = new Vector2(16, 16);
            }
        }
        if (currentInteractable != null && !PlayerLook.Instance.paused && InputManager.GetButtonDown("Interact")) currentInteractable.Interact();
    }
}