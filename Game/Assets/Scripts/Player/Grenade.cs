using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public bool exploding;
    [SerializeField] GameObject explosionDamage;
    [SerializeField] GameObject explosionEffect;

    void OnTriggerEnter(Collider other)
    {
        GameObject boomParticles = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(boomParticles, 0.4f);
    }
}
