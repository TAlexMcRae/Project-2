using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    #region Variables
    [SerializeField] int senseHor;
    [SerializeField] int senseVert;
    [SerializeField] bool invertY;

    [SerializeField] int vertMin;
    [SerializeField] int vertMax;
    float xRotation;
    #endregion

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        // gets mouse input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * senseHor;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * senseVert;

        if (invertY) { xRotation += mouseY; }
        else { xRotation -= mouseY; }

        // rotates camera with limits on X-Axis
        xRotation = Mathf.Clamp(xRotation, vertMin, vertMax);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}