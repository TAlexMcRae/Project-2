using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    [SerializeField] itemType ItemType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.itemPickup(ItemType);
            Destroy(gameObject);
        }
    }
}
