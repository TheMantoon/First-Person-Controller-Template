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
    private ItemData itemData;
    private GameObject shootButton, reloadButton;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ammo = GetComponent<ItemInteractable>().itemData.GetComponent<GunComponent>().maxAmmo;
        itemData = GetComponent<ItemInteractable>().itemData;
        shootButton = PlayerInteraction.Instance.shootButton;
        reloadButton = PlayerInteraction.Instance.reloadButton;
    }

    private void Update()
    {
        UpdateCurrentGun();
        if (currentGun == null)
        {
            if (shootButton.activeSelf == true) shootButton.SetActive(false);
            if (reloadButton.activeSelf == true) reloadButton.SetActive(false);
            return;
        }
        if (shootButton.activeSelf == false) shootButton.SetActive(true);
        if (reloadButton.activeSelf == false) reloadButton.SetActive(true);
        if (PlayerLook.Instance.paused) return;
        if (InputManager.GetButtonDown("Shoot")) Shoot();
        if (InputManager.GetButtonDown("Reload")) StartCoroutine(Reload());
    }

    private void UpdateCurrentGun()
    {
        Text ammoText = PlayerInteraction.Instance.gunAmmo;
        var item = ItemManager.Instance?.takenItem?.itemData;
        if (item == null || item != itemData)
        {
            ammoText.color = new Color(1, 1, 1, Mathf.Lerp(ammoText.color.a, 0, Time.deltaTime * 5f));
            if (isReloading) isReloading = false;
            currentGun = null;
            return;
        }
        currentGun = item.GetComponent<GunComponent>();
        if (currentGun != null)
        {
            ammoText.color = new Color(1, 1, 1, Mathf.Lerp(ammoText.color.a, 1, Time.deltaTime * 5f));
            if (ammo > currentGun.maxAmmo) ammo = currentGun.maxAmmo;
            string ammoString = $"{ammo} / {currentGun.maxAmmo} Ammo";
            ammoText.text = !isReloading ? ammoString : ammoString + " (Reloading)";
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
        if (currentGun != null)
        {
            ammo = currentGun.maxAmmo;
            isReloading = false;
        }
    }
}