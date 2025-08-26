using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // The car to follow
    public float smoothSpeed = 0.125f;
    public Vector3 offset;          // Offset from the car (set Z = -10 in Inspector)

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position = car position + offset
        Vector3 desiredPosition = target.position + offset;

        // Force Z to -10 so the camera stays in 2D space
        desiredPosition.z = -10f;

        // Smooth follow
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Keep rotation flat
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
