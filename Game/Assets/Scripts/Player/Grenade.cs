using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public bool exploding;
    [SerializeField] GameObject explosionEffect;

    void OnTriggerEnter(Collider other)
    {
        Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
