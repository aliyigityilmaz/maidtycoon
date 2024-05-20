using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float targetHorizontalAngle = 45;
    public float currentHorizontalAngle = 0;
    public float targetVerticalAngle = 30; // Starting vertical angle
    public float currentVerticalAngle = 30; // Initialize to 30 for a default starting view angle
    public float rotationSpeed = 5;
    public float mouseSensitivity = 2;
    public float maxVerticalAngle = 90; // Maximum vertical angle (60 degrees up)
    public float minVerticalAngle = 30; // Minimum vertical angle (0 degrees down)

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButton(0))
        {
            targetHorizontalAngle += mouseX * mouseSensitivity;
            targetVerticalAngle -= mouseY * mouseSensitivity; // Invert mouseY to simulate natural mouse movement
        }
        else
        {
            targetHorizontalAngle = Mathf.Round(targetHorizontalAngle / 45);
            targetHorizontalAngle *= 45;
        }

        // Clamp the vertical angle to prevent the camera from exceeding the specified limits
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, 10, 75); // Limits: 30 degrees up, 30 degrees down

        if (targetHorizontalAngle < 0)
        {
            targetHorizontalAngle += 360;
        }
        if (targetHorizontalAngle > 360)
        {
            targetHorizontalAngle -= 360;
        }

        currentHorizontalAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetHorizontalAngle, rotationSpeed * Time.deltaTime);
        currentVerticalAngle = Mathf.LerpAngle(transform.eulerAngles.x, targetVerticalAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
    }
}
