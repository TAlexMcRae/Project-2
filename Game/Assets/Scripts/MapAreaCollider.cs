using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaCollider : MonoBehaviour
{
    private List<PlayerMapArea> PlayerMapAreasList = new List<PlayerMapArea>();

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.TryGetComponent<PlayerMapArea>(out PlayerMapArea playerMapArea))
        {
            PlayerMapAreasList.Add(playerMapArea);
        }
    }
    private void OnTriggerExit(Collider collider)
    {

        if (collider.TryGetComponent<PlayerMapArea>(out PlayerMapArea playerMapArea))
        {
            PlayerMapAreasList.Remove(playerMapArea);
        }
    }

    public List<PlayerMapArea> GetPlayerMapAreasList()
    {
        return PlayerMapAreasList;
    }
}

