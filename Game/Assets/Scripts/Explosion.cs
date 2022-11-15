using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    #region Variables
    [Range(1, 5)] [SerializeField] public int bombDMG;

    [SerializeField] public GameObject explosion;
    [SerializeField] int forceAMT;

    private List<Collider> objects = new List<Collider>();
    #endregion

    private void Start()
    {

        Instantiate(explosion, transform.position, explosion.transform.rotation);

        for (int rnr = 0; rnr < objects.Count; rnr++)
        {

            objects[rnr].GetComponent<InterDamage>().inflictDamage(bombDMG);

            if (objects[rnr].CompareTag("Player"))
            {

                GameManager.instance.playerScript.pushBack = ((objects[rnr].transform.position - transform.position).normalized) * forceAMT;
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!objects.Contains(other) && other.GetComponent<InterDamage>() != null)
        {

            objects.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (objects.Contains(other) && other.GetComponent<InterDamage>() != null)
        {

            objects.Remove(other);
        }
    }
}