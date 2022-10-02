using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const float STATIC_CAMERA_Z_POSITION = -10.0f;
    private const float STATIC_CAMERA_Y_POSITION = 0.0f;

    // TODO: Set to length of Background on start, or custom in unity editor
    private float lengthOfStage = 999.9f;

    private Vector3 cameraVelocity = Vector2.zero;

    [SerializeField] [Range(0.01f, 1f)]
    private float smoothingSpeed = 0.125f;

    [SerializeField]
    private Vector3 cameraOffset;

    // Player GameObject. For simplicity, grabbing it through FindObject in start rather than unityeditor.
    [SerializeField]
    private Transform targetPlayerTransform;
    
    [SerializeField]
    private bool allowVerticalCameraMovement = true;

    [SerializeField]
    private bool allowZoomingCameraMovement = false;

    void Start()
    {
        if (this.targetPlayerTransform == null)
        {
            this.targetPlayerTransform = GameObject.FindGameObjectsWithTag(Globals.Tags.PLAYER)[0].transform;
        }
        transform.position = targetPlayerTransform.position + cameraOffset;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = this.targetPlayerTransform.position + cameraOffset;
        desiredPosition = GetDesiredPositionWithClamps(desiredPosition);
        
        // EASY and SMOOTH Camera Follow Unity Tutorial https://www.youtube.com/watch?v=OUblaHNECCI
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref cameraVelocity, smoothingSpeed);
    }

    private Vector3 GetDesiredPositionWithClamps(Vector3 desiredPosition)
    {
        // Clamp x to not go less than screen, greater than end of stage
        float potentialX = Mathf.Max(desiredPosition.x, 0.0f);
        potentialX = Mathf.Min(desiredPosition.x, this.lengthOfStage);

        Debug.Log("Potential X " + potentialX + " desired x " + desiredPosition.x);
        
        float potentialY = this.allowVerticalCameraMovement ? desiredPosition.y : STATIC_CAMERA_Y_POSITION;
        potentialY = Mathf.Max(potentialY, 0.0f);

        float potentialZ = this.allowZoomingCameraMovement ? desiredPosition.z : STATIC_CAMERA_Z_POSITION;
        return new Vector3(potentialX, potentialY, potentialZ);
    }
}