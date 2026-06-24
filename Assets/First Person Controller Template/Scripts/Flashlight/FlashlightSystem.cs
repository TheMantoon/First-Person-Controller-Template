using UnityEngine;

public class FlashlightSystem : MonoBehaviour
{
    [SerializeField] private Light flashlight = null;
    private AudioSource audioSource;
    private FlashlightComponent currentFlashlight;
    private bool enabledFlashlight;
    private float battery;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            currentFlashlight = null;
            enabledFlashlight = false;
            return;
        }
        FlashlightComponent newFlashlight = item.GetComponent<FlashlightComponent>();
        if (newFlashlight != currentFlashlight)
        {
            currentFlashlight = newFlashlight;
            if (currentFlashlight != null) battery = currentFlashlight.batteryCapacity;
            enabledFlashlight = false;
            if (flashlight != null) flashlight.enabled = false;
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