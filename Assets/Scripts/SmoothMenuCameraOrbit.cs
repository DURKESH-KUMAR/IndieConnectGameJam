using UnityEngine;

public class SmoothMenuCameraOrbit : MonoBehaviour
{
    public Transform target;              // Player character point
    public float radius = 5f;             // Distance from player
    public float height = 2f;             // Camera height
    public float rotationSpeed = 20f;     // Orbit speed
    public float smoothTime = 0.5f;       // Movement smoothness
    public float lookSmoothSpeed = 5f;    // Rotation smoothness

    private float currentAngle;
    private Vector3 velocity;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Camera Target is missing!");
            return;
        }

        // Calculate starting angle
        Vector3 direction = transform.position - target.position;
        currentAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Rotate angle around player
        currentAngle += rotationSpeed * Time.deltaTime;

        // Calculate desired camera position
        float x = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;

        Vector3 desiredPosition = new Vector3(
            target.position.x + x,
            target.position.y + height,
            target.position.z + z
        );


        // Smooth camera movement
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );


        // Smooth camera rotation
        Quaternion targetRotation = Quaternion.LookRotation(
            target.position + Vector3.up * height - transform.position
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            lookSmoothSpeed * Time.deltaTime
        );
    }
}