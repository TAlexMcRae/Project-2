using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAction : MonoBehaviour
{
    [Header("-----Melee Stats-----")]
    [SerializeField] int atkDamage;
    [SerializeField] int atkSpeed;
    bool meleeRange;
    bool canHit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (meleeRange)
        {
            StartCoroutine(hitting());
        }
    }
    IEnumerator hitting()
    {
        canHit = true;
        yield return new WaitForSeconds(atkSpeed);
        canHit = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.Damage(atkDamage);
            meleeRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meleeRange = false;
        }
    }
}
