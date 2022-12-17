using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bearswipe : MonoBehaviour
{
    [SerializeField] bossBearAI bossBearAI;
    //int damage = bossBearAI.meleeDamage;
    private void OnTriggerEnter(Collider other)
    {   
        if (other.tag == "Player")
        bossBearAI.attackDamage();
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
