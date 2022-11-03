using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int timer;
    [SerializeField] int speed;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, timer);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.Damage(damage);
        }
        Destroy(gameObject);
    }
}
