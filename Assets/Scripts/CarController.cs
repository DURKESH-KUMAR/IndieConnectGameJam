using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public enum ControlType
    {
        WASD,
        ArrowKeys
    }

    [Header("Control Settings")]
    [SerializeField] private ControlType controlType = ControlType.WASD;

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
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        switch (controlType)
        {
            case ControlType.WASD:
                horizontalInput = 0f;
                verticalInput = 0f;

                if (Input.GetKey(KeyCode.A))
                    horizontalInput = -1f;
                else if (Input.GetKey(KeyCode.D))
                    horizontalInput = 1f;

                if (Input.GetKey(KeyCode.W))
                    verticalInput = 1f;
                else if (Input.GetKey(KeyCode.S))
                    verticalInput = -1f;

                isBraking = Input.GetKey(KeyCode.LeftShift);
                break;

            case ControlType.ArrowKeys:
                horizontalInput = 0f;
                verticalInput = 0f;

                if (Input.GetKey(KeyCode.LeftArrow))
                    horizontalInput = -1f;
                else if (Input.GetKey(KeyCode.RightArrow))
                    horizontalInput = 1f;

                if (Input.GetKey(KeyCode.UpArrow))
                    verticalInput = 1f;
                else if (Input.GetKey(KeyCode.DownArrow))
                    verticalInput = -1f;

                isBraking = Input.GetKey(KeyCode.RightShift);
                break;
        }
    }

    private void HandleMotor()
    {
        float targetTorque = verticalInput * motorForce;

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
        float targetSteerAngle = horizontalInput * maxSteerAngle;

        currentSteerAngle = Mathf.Lerp(
            currentSteerAngle,
            targetSteerAngle,
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