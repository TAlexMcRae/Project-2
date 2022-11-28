using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    #region Variables
    [Range(1, 5)] [SerializeField] public int bombDMG;
    [SerializeField] int forceAMT;
    private bool damaged = false;
    #endregion

    void Start()
    {

        DamageOverlay();
        Destroy(gameObject, 0.05f);
    }

    void DamageOverlay()
    {

        Collider[] objects = Physics.OverlapSphere(transform.position, 15);

        for (int rnr = 0; rnr < objects.Length; rnr++)
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