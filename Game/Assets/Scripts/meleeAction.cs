using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAction : MonoBehaviour
{
    [SerializeField] int atkDamage;
    bool meleeRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
