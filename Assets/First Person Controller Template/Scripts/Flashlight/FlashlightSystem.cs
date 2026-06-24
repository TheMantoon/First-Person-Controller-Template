using UnityEngine;
using UnityEngine.UI;

public class FlashlightSystem : MonoBehaviour
{
    [SerializeField] private Light flashlight = null;
    private AudioSource audioSource = null;
    private FlashlightComponent currentFlashlight = null;
    private bool enabledFlashlight = false;
    private float battery = 0;
    private ItemData itemData;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        battery = GetComponent<ItemInteractable>().itemData.GetComponent<FlashlightComponent>().batteryCapacity;
        itemData = GetComponent<ItemInteractable>().itemData;
        if (flashlight != null) flashlight.enabled = false;
    }

    private void Update()
    {
        UpdateCurrentFlashlight();
        if (currentFlashlight == null)
        {
            if (flashlight != null) flashlight.enabled = false;
            return;
        }
        if (PlayerLook.Instance.paused) return;
        if (InputManager.GetButtonDown("Flashlight")) ToggleFlashlight();
        UpdateBattery();
        UpdateLightSettings();
    }

    private void UpdateCurrentFlashlight()
    {
        var item = ItemManager.Instance?.takenItem?.itemData;
        Text batteryText = PlayerInteraction.Instance.flashlightBattery;
        if (item == null || item != itemData)
        {
            batteryText.color = new Color(1, 1, 1, Mathf.Lerp(batteryText.color.a, 0, Time.deltaTime * 5f));
            currentFlashlight = null;
            enabledFlashlight = false;
            return;
        }
        currentFlashlight = item.GetComponent<FlashlightComponent>();
        if (currentFlashlight != null)
        {
            if (currentFlashlight.useBattery)
            {
                batteryText.text = $"{battery:F0} / {currentFlashlight.batteryCapacity}";
                batteryText.color = new Color(1, 1, 1, Mathf.Lerp(batteryText.color.a, 1, Time.deltaTime * 5f));
            }
            else if (!currentFlashlight.useBattery)
            {
                batteryText.color = new Color(1, 1, 1, Mathf.Lerp(batteryText.color.a, 0, Time.deltaTime * 5f));
                return;
            }
        }
    }

    private void ToggleFlashlight()
    {
        if (currentFlashlight == null) return;
        if (currentFlashlight.useBattery && battery <= 0) return;
        enabledFlashlight = !enabledFlashlight;
        if (flashlight != null) flashlight.enabled = enabledFlashlight;
        AudioClip clip = enabledFlashlight ? currentFlashlight.turnOnSound : currentFlashlight.turnOffSound;
        if (clip != null) audioSource.PlayOneShot(clip);
    }

    private void UpdateBattery()
    {
        if (!enabledFlashlight) return;
        if (!currentFlashlight.useBattery) return;
        battery -= currentFlashlight.batteryDrainPerSecond * Time.deltaTime;
        if (battery <= 0)
        {
            battery = 0;
            enabledFlashlight = false;
            if (flashlight != null) flashlight.enabled = false;
            if (currentFlashlight.turnOffSound) audioSource.PlayOneShot(currentFlashlight.turnOffSound);
        }
    }

    private void UpdateLightSettings()
    {
        if (flashlight == null) return;
        flashlight.color = currentFlashlight.lightColor;
        flashlight.intensity = currentFlashlight.intensity;
        flashlight.range = currentFlashlight.range;
        flashlight.spotAngle = currentFlashlight.spotAngle;
    }

    public float GetBattery01()
    {
        if (currentFlashlight == null) return 0f;
        if (!currentFlashlight.useBattery) return 1f;
        return battery / currentFlashlight.batteryCapacity;
    }

    public bool IsEnabled => enabledFlashlight;
}