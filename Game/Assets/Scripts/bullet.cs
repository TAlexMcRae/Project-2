using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int timer;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, timer); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            GameManager.instance.playerScript.Damage(damage);
        }
        Destroy(gameObject);
    }
}
