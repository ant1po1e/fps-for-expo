using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;

    public AudioClip m1911Shot;
    public AudioClip m4Shot;

    public AudioSource m4Reload;
    public AudioSource m1911Reload;

    public AudioSource emptyMagazine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                ShootingChannel.PlayOneShot(m4Shot);
                break;
            case WeaponModel.M4:
                ShootingChannel.PlayOneShot(m4Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                m1911Reload.Play();
                break;
            case WeaponModel.M4:
                m4Reload.Play();
                break;
        }
    }
}
