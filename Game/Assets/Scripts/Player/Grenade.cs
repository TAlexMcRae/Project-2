using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public bool exploding;
    [SerializeField] GameObject explosionDamage;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] AudioSource audi;
    [SerializeField] AudioClip explosionSFX;

    void OnTriggerEnter(Collider other)
    {
        audi.PlayOneShot(explosionSFX);
        GameObject boomParticles = Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        GameObject boomDamage = Instantiate(explosionDamage, gameObject.transform.position, gameObject.transform.rotation);

        Destroy(boomParticles, 0.4f);
        Destroy(boomDamage, 0.5f);
    }
}
