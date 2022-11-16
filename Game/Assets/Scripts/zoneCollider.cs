using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneCollider : MonoBehaviour
{
    public int captureCount;
    [SerializeField] Renderer cube, cube1, cube2, cube3;
    public GameObject signObject;
    private bool isCapturing = false;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Capture());

            signObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine(Capture());
            signObject.SetActive(false);
            captureCount = +1;
        }
    }

    IEnumerator Capture()
    {
        isCapturing = true;
        //Remember to set it for 1 second less
        yield return new WaitForSeconds(4);
        cube.material.color = Color.cyan;
        cube1.material.color = Color.cyan;
        cube2.material.color = Color.cyan;
        cube3.material.color = Color.cyan;
        isCapturing = false;
    }
}