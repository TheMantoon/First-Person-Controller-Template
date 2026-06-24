using UnityEngine;
using UnityEngine.UI;

public class FlashlightSystem : MonoBehaviour
{
    [SerializeField] private Light flashlight = null;
    private AudioSource audioSource = null;
    private FlashlightComponent currentFlashlight = null;
    private bool enabledFlashlight = false;
    private float battery = 0;
    private bool itemState = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        battery = GetComponent<ItemInteractable>().itemData.GetComponent<FlashlightComponent>().batteryCapacity;
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
        if (item == null)
        {
            if (itemState)
            {
                PlayerInteraction.Instance.needItemInfo = false;
                itemState = false;
            }
            currentFlashlight = null;
            enabledFlashlight = false;
            return;
        }
        currentFlashlight = item.GetComponent<FlashlightComponent>();
        if (currentFlashlight != null)
        {
            if ((!itemState || !PlayerInteraction.Instance.needItemInfo) && currentFlashlight.useBattery)
            {
                PlayerInteraction.Instance.needItemInfo = true;
                itemState = true;
            }
            Text batteryText = PlayerInteraction.Instance.itemInfo;
            batteryText.text = $"{battery.ToString("F0")} / {currentFlashlight.batteryCapacity}";
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