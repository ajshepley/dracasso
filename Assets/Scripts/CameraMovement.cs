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

    // Change in unity editor. Higher value = camera is slower to catch up.
    [SerializeField] [Range(0.01f, 1f)]
    private float smoothingSpeed = 0.75f;

    [SerializeField]
    private Vector3 cameraOffset = new Vector3(0.0f, 3.0f, 0.0f);

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
        potentialX = Mathf.Min(potentialX, this.lengthOfStage);
        
        float potentialY = Mathf.Max(desiredPosition.y, 0.0f);
        potentialY = this.allowVerticalCameraMovement ? potentialY : STATIC_CAMERA_Y_POSITION;

        float potentialZ = this.allowZoomingCameraMovement ? desiredPosition.z : STATIC_CAMERA_Z_POSITION;
        return new Vector3(potentialX, potentialY, potentialZ);
    }
}