using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region
    public static WeaponManager Instance { get; set; }

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
    #endregion


    public bool activeWeapon;

    public GameObject m1911Mode;
    public GameObject m4Model;
    public GameObject ak47Model;
    public GameObject bennelliModel;

    private void Start()
    {
        m1911Mode.SetActive(false);
        m4Model.SetActive(false);
        ak47Model.SetActive(false);
        bennelliModel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeWeapon = true;

            m1911Mode.SetActive(true);
            m4Model.SetActive(false);
            ak47Model.SetActive(false);
            bennelliModel.SetActive(false);
        }
        else
        {
            activeWeapon = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeWeapon = true;

            m1911Mode.SetActive(false);
            m4Model.SetActive(true);
            ak47Model.SetActive(false);
            bennelliModel.SetActive(false);
        }
        else
        {
            activeWeapon = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeWeapon = true;

            m1911Mode.SetActive(false);
            m4Model.SetActive(false);
            ak47Model.SetActive(true);
            bennelliModel.SetActive(false);
        }
        else
        {
            activeWeapon = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            activeWeapon = true;

            m1911Mode.SetActive(false);
            m4Model.SetActive(false);
            ak47Model.SetActive(false);
            bennelliModel.SetActive(true);
        }
        else
        {
            activeWeapon = false;
        }
    }
}