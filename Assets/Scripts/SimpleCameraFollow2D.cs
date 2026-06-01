using UnityEngine;

public class SimpleCameraFollow2D : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.15f;

    // Fixed 2D camera distance
    public float cameraZ = -10f;

    public Vector2 offset = new Vector2(0f, 1f);

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Build the desired position
        Vector3 desiredPos = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            cameraZ
        );

        // Smooth movement
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref velocity,
            smoothSpeed
        );
    }
}
