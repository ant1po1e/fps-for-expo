using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public static Bottle Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<Rigidbody> allParts = new List<Rigidbody>();

    public void Shatter()
    {
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;
        }

        Destroy(gameObject, 1f);
    }
}
