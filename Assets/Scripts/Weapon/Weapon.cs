using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Shooting Variables
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    #endregion

    public int bulletPerBurst = 3;
    public int burstBulletLeft;

    public float spreadIntensity;

    #region Bullet Variables
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;
    #endregion

    public GameObject muzzleEffect;
    private Animator anim;

    #region Reload Variables
    public float reloadTime;
    public int magazineSize, bulletLeft;
    public bool isReloading;
    #endregion

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
    }

    void Update()
    {
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

        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletLeft / bulletPerBurst}/{magazineSize / bulletPerBurst}";
        }
    }

    private void FireWeapon()
    {
        bulletLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        anim.SetTrigger("RECOIL");

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

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);


        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
