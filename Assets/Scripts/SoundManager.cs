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
        public AudioClip ak47Shot;
        public AudioClip bennelliShot;

        public AudioSource m4Reload;
        public AudioSource m1911Reload;
        public AudioSource ak47Reload;
        public AudioSource bennelliReload;

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
                    ShootingChannel.PlayOneShot(m1911Shot);
                    break;
                case WeaponModel.M4:
                    ShootingChannel.PlayOneShot(m4Shot);
                    break;
                case WeaponModel.AK47:
                    ShootingChannel.PlayOneShot(ak47Shot);
                    break;
                case WeaponModel.Bennelli:
                    ShootingChannel.PlayOneShot(bennelliShot);
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
                case WeaponModel.AK47:
                    ak47Reload.Play();
                    break;
                case WeaponModel.Bennelli:
                    bennelliReload.Play();
                    break;
            }
        }
    }
