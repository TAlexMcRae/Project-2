using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneCollider : MonoBehaviour
{
    [SerializeField] Renderer cube, cube1, cube2, cube3;
    private bool isCapturing = false;
    private bool captured;
    //THIS NUMBER IS IMPORTANT FOR THE WIN CONDITION
    private int numberOfZones;

    void Start()
    {

        numberOfZones = 5;
        captured = false;
    }

    private void Update()
    {

        if (GameManager.instance.enemiesToKill == 0 && GameManager.instance.capturedAll)
        {

            GameManager.instance.captureCount = 0;
            GameManager.instance.capturedAll = false;
            cube.material.color = Color.white;
            cube1.material.color = Color.white;
            cube2.material.color = Color.white;
            cube3.material.color = Color.white;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Capture());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine(Capture());
        }
    }

    IEnumerator Capture()
    {
        VerifyIfCapturedAll();
        isCapturing = true;
        //Remember to set it for 1 second less
        yield return new WaitForSeconds(9);
        cube.material.color = Color.cyan;
        cube1.material.color = Color.cyan;
        cube2.material.color = Color.cyan;
        cube3.material.color = Color.cyan;
        if (captured == false)
        {
            GameManager.instance.captureCount++;
            GameManager.instance.UpdateUI();
        }
        captured = true;
        isCapturing = false;
    }

    private void VerifyIfCapturedAll()
    {
        if (GameManager.instance.captureCount == numberOfZones)
            GameManager.instance.capturedAll = true;
    }
}