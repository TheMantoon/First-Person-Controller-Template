using UnityEngine;
using System;

[Serializable]
public class GunComponent : ItemComponent
{
    public AudioClip shootSound;
    public AudioClip noAmmoSound;
    public AudioClip reloadSound;
    public int maxAmmo = 6;
    public float damage = 15;
    public float reloadTime = 1;
}