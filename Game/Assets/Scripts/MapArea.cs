using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    #region variables

    //To count the amount of areas captured, important for the victory bool in GameManager
    public int captureCount;

    //To change the colour of the cube (visual representation of a captured area)
    [SerializeField] Renderer cube;

    #endregion

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
                cube.material.color = Color.white;
                float progressSpeed = 1f;
                progressOfCapture += progressSpeed * Time.deltaTime;
                if (progressOfCapture >= 7)
                    state = State.Captured;
                break;
            case State.Captured:
                cube.material.color = Color.cyan;
                captureCount =+ 1;
                break;
        }
        if (captureCount >= 1)
            GameManager.instance.capturedAll = true;
    }
}
