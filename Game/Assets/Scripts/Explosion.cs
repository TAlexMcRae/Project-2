using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    #region Variables
    [Range(1, 5)] [SerializeField] public int bombDMG;

    [SerializeField] public GameObject explosion;
    [SerializeField] int forceAMT;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<InterDamage>() != null)
        {

            Instantiate(explosion, transform.position, explosion.transform.rotation);
            other.GetComponent<InterDamage>().inflictDamage(bombDMG);

            if (other.CompareTag("Player"))
            {

                GameManager.instance.playerScript.pushBack = ((other.transform.position - transform.position).normalized) * forceAMT;
            }

            Destroy(gameObject);
        }
    }
}