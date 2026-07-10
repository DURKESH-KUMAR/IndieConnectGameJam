using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private bool isBraking;

    private float currentSteerAngle;
    private float currentMotorTorque;
    private float currentBrakeForce;

    [Header("Car Settings")]
    [SerializeField] private float motorForce = 2000f;
    [SerializeField] private float brakeForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;

    [Header("Smoothness")]
    [SerializeField] private float steeringSpeed = 6f;
    [SerializeField] private float accelerationSpeed = 5f;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Makes physics movement much smoother
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        // Read input every frame
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        float targetTorque = verticalInput * motorForce;

        // Smooth acceleration
        currentMotorTorque = Mathf.Lerp(
            currentMotorTorque,
            targetTorque,
            accelerationSpeed * Time.fixedDeltaTime);

        frontLeftWheelCollider.motorTorque = currentMotorTorque;
        frontRightWheelCollider.motorTorque = currentMotorTorque;

        currentBrakeForce = isBraking ? brakeForce : 0f;

        ApplyBrakes();
    }

    private void ApplyBrakes()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        float targetAngle = horizontalInput * maxSteerAngle;

        // Smooth steering
        currentSteerAngle = Mathf.Lerp(
            currentSteerAngle,
            targetAngle,
            steeringSpeed * Time.fixedDeltaTime);

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider collider, Transform wheel)
    {
        collider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        wheel.position = position;
        wheel.rotation = rotation;
    }
}