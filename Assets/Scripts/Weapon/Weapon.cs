using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Shooting")]
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Bullet Burst")]
    public int bulletPerBurst = 3;
    public int burstBulletLeft;

    [Header("Spread Intensity")]
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;
    private float spreadIntensity;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    [Header("Reloading")]
    public GameObject muzzleEffect;
    private Animator anim;
    public float reloadTime;
    public int magazineSize, bulletLeft;
    public bool isReloading;

    public bool isADS;

    public enum WeaponModel
    {
        M1911,
        M4,
        AK47,
        Bennelli
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single, Burst, Auto
    }

    public ShootingMode currentMode;

    public void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletPerBurst;
        anim = GetComponent<Animator>();

        bulletLeft = magazineSize;
        spreadIntensity = hipSpreadIntensity;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            HUDManager.Instance.crossair.SetActive(false);
            anim.SetTrigger("enterADS");
            isADS = true;
            spreadIntensity = adsSpreadIntensity;
        }
        if (Input.GetMouseButtonUp(1))
        {
            HUDManager.Instance.crossair.SetActive(true);
            anim.SetTrigger("exitADS");
            isADS = false;
            spreadIntensity = hipSpreadIntensity;
        }

        if (bulletLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazine.Play();
        }

        if (currentMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentMode == ShootingMode.Single || currentMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletLeft < magazineSize && isReloading == false)
        {
            Reload();
        }

        if (readyToShoot && isShooting && bulletLeft > 0)
        {
            burstBulletLeft = bulletPerBurst;
            FireWeapon();
        }

        if (HUDManager.Instance.magazineAmmoUI != null)
        {
            HUDManager hudManager = HUDManager.Instance;

            hudManager.magazineAmmoUI.text = $"{bulletLeft / bulletPerBurst}";
            hudManager.totalAmmoUI.text = $"{magazineSize / bulletPerBurst}";

            WeaponModel model = thisWeaponModel;

            hudManager.activeWeaponUI.sprite = GetWeaponSprite(model);
        }
    }

    private void FireWeapon()
    {
        bulletLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        if (isADS)
        {
            anim.SetTrigger("adsRECOIL");
        }
        else
        {
            anim.SetTrigger("RECOIL");
        }

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootigDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootigDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootigDirection * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBullet(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentMode == ShootingMode.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        anim.SetTrigger("RELOAD");

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float Z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);


        return direction + new Vector3(0, y, Z);
    }

    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    private Sprite GetWeaponSprite(WeaponModel model)
    {
        switch (model)
        {
            case WeaponModel.M1911:
                return Instantiate(Resources.Load<GameObject>("M1911_Weapon")).GetComponent<SpriteRenderer>().sprite;

            case WeaponModel.M4:
                return Instantiate(Resources.Load<GameObject>("M4_Weapon")).GetComponent<SpriteRenderer>().sprite;

            case WeaponModel.AK47:
                return Instantiate(Resources.Load<GameObject>("AK47_Weapon")).GetComponent<SpriteRenderer>().sprite;

            case WeaponModel.Bennelli:
                return Instantiate(Resources.Load<GameObject>("Benneli_Weapon")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }
}
