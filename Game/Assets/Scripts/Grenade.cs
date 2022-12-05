using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    [Header("Variables")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject grenade;

    [Header("Settings")]
    public int grenadeCounter;
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;
    bool throwReady;

    public bool exploding;
    [SerializeField] GameObject explosionEffect;

    [Header("Explosion")]
    public float radius = 15f;
    public float explosionForce = 20f;


    // Start is called before the first frame update
    void Start()
    {
        exploding = false;
        throwReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Grenade") && throwReady && grenadeCounter > 0)
        {
            Throw();
        }
    }

    void Throw()
    {
        throwReady = false;
        GameObject projectile = Instantiate(grenade, attackPoint.position, cam.rotation);
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        grenadeCounter--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        throwReady = true;
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider near in colliders)
        {
            Rigidbody rig = near.GetComponent<Rigidbody>();
            if(rig!= null)
            rig.AddExplosionForce(explosionForce, transform.position, radius, 1f, ForceMode.Impulse);
        }
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    /*IEnumerator Explode()
    {
        exploding = true;
        yield return InterDamage;

        Instantiate(grenade, gameObject.transform.position, gameObject.transform.rotation);
        Instantiate(explosionEffect, gameObject.transform.position, gameObject.transform.rotation);
        int chance = Random.Range(1, 3);

        Destroy(gameObject);
    }*/
}
