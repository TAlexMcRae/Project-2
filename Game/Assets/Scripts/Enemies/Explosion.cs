using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    #region Variables
    [Range(1, 5)] [SerializeField] public int bombDMG;
    [SerializeField] public int forceAMT;
    private bool damaged = false;
    private float explosionRadius;
    #endregion

    void Start()
    {

        if (PlayerPref.mediumMode)
        {

            bombDMG *= 2;
            forceAMT *= 2;
        }

        else if (PlayerPref.hardMode)
        {

            bombDMG *= 4;
            forceAMT *= 2;
        }

        //Too big for the grenade
        explosionRadius = 15;
        DamageOverlay();
    }

    void DamageOverlay()
    {

        Collider[] objects = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int rnr = 0; rnr < objects.Length; rnr++)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitinfo, 15f))
            {

                if (objects[rnr].GetComponent<InterDamage>() != null)
                {

                objects[rnr].GetComponent<InterDamage>().inflictDamage(bombDMG);

                if (!damaged && objects[rnr].CompareTag("Player"))
                    {
                    
                    damaged = true;
                    GameManager.instance.playerScript.pushBack = ((objects[rnr].transform.position - transform.position).normalized) * forceAMT;
                    }
                }
            }
        }
    }
}