using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            HandleTargetCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Bottle10") || collision.gameObject.CompareTag("Bottle20") || collision.gameObject.CompareTag("Bottle30") || collision.gameObject.CompareTag("Bottle40") || collision.gameObject.CompareTag("Bottle50"))
        {
            HandleBottleCollision(collision);
        }
    }

    void HandleTargetCollision(Collision collision)
    {
        print("hit " + collision.gameObject.name + "!");
        BulletImpactEffect(collision);
        Destroy(gameObject);
    }

    void HandleWallCollision(Collision collision)
    {
        print("hit a wall");
        BulletImpactEffect(collision);
        Destroy(gameObject);
    }

    void HandleBottleCollision(Collision collision)
    {
        Bottle bottle = collision.gameObject.GetComponent<Bottle>();
        if (bottle != null)
        {
            bottle.Shatter();
            int pointValue = GetPointValueFromTag(collision.gameObject.tag);
            ScoreManager.instance.AddPoint(pointValue);
        }
        Destroy(gameObject);
    }

    int GetPointValueFromTag(string tag)
    {
        switch (tag)
        {
            case "Bottle10":
                return 10;
            case "Bottle20":
                return 20;
            case "Bottle30":
                return 30;
            case "Bottle40":
                return 40;
            case "Bottle50":
                return 50;
            default:
                return 0; // Return 0 if tag is not recognized
        }
    }

    void BulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject hole = Instantiate(
            GlobalReference.Instance.bulletImpactPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        hole.transform.SetParent(collision.gameObject.transform);
    }
}