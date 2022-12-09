using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] public int damage;
    [SerializeField] int timer;
    [SerializeField] public int speed;

    private void Start()
    {

        if (PlayerPref.mediumMode)
        {

            damage *= 2;
            speed *= 2;
        }

        else if (PlayerPref.hardMode)
        {

            damage *= 4;
            speed *= 4;
        }

        rb.velocity = transform.forward * speed;
        Destroy(gameObject, timer);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.GetComponent<InterDamage>().inflictDamage(damage);
        }
        Destroy(gameObject);
    }
}
