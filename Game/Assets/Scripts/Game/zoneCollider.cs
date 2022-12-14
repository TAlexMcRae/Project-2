using System.Collections;
using UnityEngine;

public class zoneCollider : MonoBehaviour
{
    [SerializeField] Renderer cube, cube1, cube2, cube3;
    [SerializeField] AudioSource audi;
    [SerializeField] AudioClip capturedZone;
    [SerializeField] AudioClip capturingZone;
    [SerializeField] AudioClip allZonesCaptured;

    private bool isCapturing = false;
    public bool captured = false;

    private Color freeColor = Color.white;
    private Color capColor = Color.cyan;

    public Coroutine temp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !captured && temp == null)
        {
            temp = StartCoroutine(Capture());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !captured)
        {
            StopCoroutine(temp);
            GameManager.instance.capturing.SetActive(false);
            GameManager.instance.timerText.SetActive(false);
            temp = null;
        }
    }

    IEnumerator Capture()
    {

        isCapturing = true;
        audi.PlayOneShot(capturingZone);
        GameManager.instance.capturing.SetActive(true);
        GameManager.instance.timerText.SetActive(true);
        GameManager.instance.secondsLeft = 5.1f;

        // set to 1 seconds under
        yield return new WaitForSeconds(5f);
        ColorChange();

        GameManager.instance.capturing.SetActive(false);
        GameManager.instance.timerText.SetActive(false);

        captured = true;
        isCapturing = false;
        audi.PlayOneShot(capturedZone);
        GameManager.instance.captureCount--;
        GameManager.instance.UpdateUI();

        if (GameManager.instance.captureCount <= 0)
        {

            GameManager.instance.capturedAll = true;
            audi.PlayOneShot(allZonesCaptured);
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