using UnityEngine;
using System;

[Serializable]
public class FlashlightComponent : ItemComponent
{
    [Header("Light")]
    public Color lightColor = Color.white;
    public float intensity = 2f;
    public float range = 20f;
    public float spotAngle = 60f;

    [Header("Battery")]
    public bool useBattery = false;
    public float batteryCapacity = 120f;
    public float batteryDrainPerSecond = 1f;

    [Header("Sounds")]
    public AudioClip turnOnSound;
    public AudioClip turnOffSound;
}