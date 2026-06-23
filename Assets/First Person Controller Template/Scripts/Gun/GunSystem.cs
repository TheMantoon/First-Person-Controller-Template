using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    [SerializeField] private float shootDistance = 100f;
    [SerializeField] private LayerMask hitMask = ~0;
    [SerializeField] private BulletVisual bulletPrefab = null;
    [SerializeField] private float bulletSpeed = 80f;
    private AudioSource audioSource = null;
    private int ammo = 0;
    private GunComponent currentGun = null;
    private bool isReloading = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        UpdateCurrentGun();
        if (currentGun == null) return;
        if (PlayerLook.Instance.paused) return;
        if (InputManager.GetButtonDown("Shoot")) Shoot();
        if (InputManager.GetButtonDown("Reload")) StartCoroutine(Reload());
    }

    private void UpdateCurrentGun()
    {
        var item = ItemManager.Instance?.takenItem?.itemData;
        Text ammoText = PlayerInteraction.Instance.ammoText;
        if (item == null)
        {
            ammoText.color = new Color(1, 1, 1, Mathf.Lerp(ammoText.color.a, 0, Time.deltaTime * 5f));
            if (isReloading) isReloading = false;
            currentGun = null;
            return;
        }
        currentGun = item.GetComponent<GunComponent>();
        if (currentGun != null)
        {
            if (ammo > currentGun.maxAmmo) ammo = currentGun.maxAmmo;
            string ammoString = $"{ammo} / {currentGun.maxAmmo} Ammo";
            ammoText.text = !isReloading ? ammoString : ammoString + " (Reloading)";
            ammoText.color = new Color(1, 1, 1, Mathf.Lerp(ammoText.color.a, 1, Time.deltaTime * 5f));
        }
    }

    private void Shoot()
    {
        if (isReloading) return;
        if (ammo <= 0)
        {
            if (currentGun.noAmmoSound) audioSource.PlayOneShot(currentGun.noAmmoSound);
            return;
        }
        ammo--;
        if (currentGun.shootSound) audioSource.PlayOneShot(currentGun.shootSound);
        Vector3 hitPoint = PlayerController.Instance.cameraPivot.position + PlayerController.Instance.cameraPivot.forward * shootDistance;
        if (Physics.Raycast(PlayerController.Instance.cameraPivot.position, PlayerController.Instance.cameraPivot.forward,
            out RaycastHit hit, shootDistance, hitMask))
        {
            hitPoint = hit.point;
            var damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null) damageable.TakeDamage(currentGun.damage);
        }
        BulletVisual bullet = Instantiate(bulletPrefab, PlayerController.Instance.cameraPivot.position, Quaternion.identity);
        bullet.Init(hitPoint, bulletSpeed);
    }

    private IEnumerator Reload()
    {
        if (isReloading) yield break;
        if (ammo == currentGun.maxAmmo) yield break;
        isReloading = true;
        if (currentGun.reloadSound) audioSource.PlayOneShot(currentGun.reloadSound);
        yield return new WaitForSeconds(currentGun.reloadTime);
        ammo = currentGun.maxAmmo;
        isReloading = false;
    }
}