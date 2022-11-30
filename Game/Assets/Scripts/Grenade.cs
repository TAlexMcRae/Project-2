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


    // Start is called before the first frame update
    void Start()
    {
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

        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        grenadeCounter--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        throwReady = true;
    } 
}
