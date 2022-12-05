using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class zoneCollider : MonoBehaviour
{
    [SerializeField] Renderer cube, cube1, cube2, cube3;

    private bool isCapturing;
    public bool captured;

    private Color freeColor = Color.white;
    private Color capColor = Color.cyan;

    public float captureTime;

    void Start()
    {

        isCapturing = false;
        captured = false;
        captureTime = 5.1f;
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
            GameManager.instance.capturing.SetActive(false);
            GameManager.instance.timerText.SetActive(false);
        }
    }

    IEnumerator Capture()
    {

        isCapturing = true;

        GameManager.instance.capturing.SetActive(true);
        GameManager.instance.timerText.SetActive(true);
        GameManager.instance.secondsLeft = captureTime;

        // set to 1 seconds under
        yield return new WaitForSeconds(5f);
        ColorChange();

        GameManager.instance.capturing.SetActive(false);
        GameManager.instance.timerText.SetActive(false);

        captured = true;
        isCapturing = false;

        GameManager.instance.captureCount--;
        GameManager.instance.UpdateUI();

        if (GameManager.instance.captureCount <= 0)
        {

            GameManager.instance.capturedAll = true;

            if (GameManager.instance.enemiesToKill <= 0 && SceneManager.GetActiveScene().buildIndex < 4)
            {

                GameManager.instance.AdvancePrompt();
            }
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