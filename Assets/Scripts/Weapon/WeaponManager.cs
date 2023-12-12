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

    public GameObject m1911Model;
    public GameObject m4Model;
    public GameObject ak47Model;
    public GameObject bennelliModel;

    private void Start()
    {
        m1911Model.SetActive(false);
        m4Model.SetActive(false);
        ak47Model.SetActive(false);
        bennelliModel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m1911Model.SetActive(true);
            m4Model.SetActive(false);
            ak47Model.SetActive(false);
            bennelliModel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m1911Model.SetActive(false);
            m4Model.SetActive(true);
            ak47Model.SetActive(false);
            bennelliModel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m1911Model.SetActive(false);
            m4Model.SetActive(false);
            ak47Model.SetActive(true);
            bennelliModel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m1911Model.SetActive(false);
            m4Model.SetActive(false);
            ak47Model.SetActive(false);
            bennelliModel.SetActive(true);
        }
    }
}
