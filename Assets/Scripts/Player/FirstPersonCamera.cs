using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    // Variables
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform playerObj;

    float xRotation;
    float yRotation;


    void Start()
    {
        // Lock and Hide the Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        // Collect Mouse Input

        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate the Camera around its local X axis
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        playerObj.rotation = Quaternion.Euler(0, yRotation, 0);
        //playerObj.Rotate(Vector3.up * mouseX);
    }
}