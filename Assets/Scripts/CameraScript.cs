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
    public float maxVerticalAngle = 75; // Maximum vertical angle (75 degrees up)
    public float minVerticalAngle = 10; // Minimum vertical angle (10 degrees down)
    public float maxHorizontalAngle = 90; // Maximum horizontal angle (90 degrees right)
    public float minHorizontalAngle = -90; // Minimum horizontal angle (90 degrees left)
    public float zoomSpeed = 2; // Speed of zooming
    public float maxZoom = 40; // Maximum zoom (field of view)
    public float minZoom = 10; // Minimum zoom (field of view)

    private Camera cameraComponent;

    void Start()
    {
        cameraComponent = GetComponentInChildren<Camera>();
        if (cameraComponent == null)
        {
            Debug.LogError("No camera found in children of CameraScript object.");
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButton(0))
        {
            targetHorizontalAngle += mouseX * mouseSensitivity;
            targetVerticalAngle -= mouseY * mouseSensitivity; // Invert mouseY to simulate natural mouse movement
        }
        else
        {
            targetHorizontalAngle = Mathf.Round(targetHorizontalAngle / 45) * 45;
        }

        // Clamp the vertical angle to prevent the camera from exceeding the specified limits
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, minVerticalAngle, maxVerticalAngle);

        // Clamp the horizontal angle within the range [minHorizontalAngle, maxHorizontalAngle]
        targetHorizontalAngle = Mathf.Clamp(targetHorizontalAngle, minHorizontalAngle, maxHorizontalAngle);

        currentHorizontalAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetHorizontalAngle, rotationSpeed * Time.deltaTime);
        currentVerticalAngle = Mathf.LerpAngle(transform.eulerAngles.x, targetVerticalAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);

        // Zoom functionality
        if (cameraComponent != null)
        {
            float targetZoom = cameraComponent.fieldOfView - scroll * zoomSpeed * 100 * Time.deltaTime;
            cameraComponent.fieldOfView = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }
}
