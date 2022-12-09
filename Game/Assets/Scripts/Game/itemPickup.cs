using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickup : MonoBehaviour
{
    [SerializeField] itemType ItemType;
    [SerializeField] float spinRate;

    private void Update()
    {

        transform.Rotate(spinRate * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.itemPickup(ItemType);
            Destroy(gameObject);
        }
    }
}
