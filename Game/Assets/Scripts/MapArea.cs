using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    public int captureCount;
    public bool capturedAll = false;

    public enum State
    {
        Neutral,
        Captured,
    }

    private List<MapAreaCollider> mapAreaColliderList;
    private State state;
    private float progressOfCapture;

    private void Awake()
    {
        mapAreaColliderList = new List<MapAreaCollider>();

        foreach (Transform child in transform)
        {
            MapAreaCollider mapAreaCollider = child.GetComponent<MapAreaCollider>();
            if (mapAreaCollider != null)
            {
                mapAreaColliderList.Add(mapAreaCollider);
            }
        }
        state = State.Neutral;
    }


    private void Update()
    {
        switch (state)
        {
            case State.Neutral:
                float progressSpeed = 1f;
                progressOfCapture += progressSpeed * Time.deltaTime;
                if (progressOfCapture >= 10)
                    state = State.Captured;
                break;
            case State.Captured:
                captureCount++;
                break;
        }
        if (captureCount >= 1)
            capturedAll = true;
    }
}
