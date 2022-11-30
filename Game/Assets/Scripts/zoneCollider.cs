using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneCollider : MonoBehaviour
{
    [SerializeField] Renderer cube, cube1, cube2, cube3;

    private bool isCapturing;
    public bool captured;

    private Color freeColor = Color.white;
    private Color capColor = Color.cyan;

    void Start()
    {

        isCapturing = false;
        captured = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !captured)
        {
            StartCoroutine(Capture());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !captured)
        {
            StopCoroutine(Capture());
        }
    }

    IEnumerator Capture()
    {

        isCapturing = true;

        // set to 1 seconds under
        yield return new WaitForSeconds(9);
        ColorChange();

        captured = true;
        isCapturing = false;

        GameManager.instance.captureCount--;
        GameManager.instance.UpdateUI();

        if (GameManager.instance.captureCount <= 0)
        {

            GameManager.instance.capturedAll = true;
        }
    }

    public void ColorChange()
    {

        if (cube.material.color == freeColor)
        {

            cube.material.color = capColor;
            cube1.material.color = capColor;
            cube2.material.color = capColor;
            cube3.material.color = capColor;
        }

        else if (cube.material.color == capColor)
        {

            cube.material.color = freeColor;
            cube1.material.color = freeColor;
            cube2.material.color = freeColor;
            cube3.material.color = freeColor;
        }
    }
}