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

    [Header("Car Settings")]
    [SerializeField] private float motorForce = 1800f;
    [SerializeField] private float brakeForce = 4000f;
    [SerializeField] private float maxSteerAngle = 30f;

    [Header("Smoothness")]
    [SerializeField] private float steeringSpeed = 8f;

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

    [Header("Spawner")]
    [SerializeField] private PlatformSpawner platformSpawner;

    private Rigidbody rb;

    private float horizontalInput;
    private bool isBraking;
    private float currentSteerAngle;

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
        horizontalInput = 0f;

        switch (controlType)
        {
            case ControlType.WASD:

                if (Input.GetKey(KeyCode.A))
                    horizontalInput = -1f;
                else if (Input.GetKey(KeyCode.D))
                    horizontalInput = 1f;

                break;

            case ControlType.ArrowKeys:

                if (Input.GetKey(KeyCode.LeftArrow))
                    horizontalInput = -1f;
                else if (Input.GetKey(KeyCode.RightArrow))
                    horizontalInput = 1f;

                break;
        }

        // Space Bar brakes in both control modes
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        if (isBraking)
        {
            ApplyBrake(brakeForce);

            frontLeftWheelCollider.motorTorque = 0f;
            frontRightWheelCollider.motorTorque = 0f;
        }
        else
        {
            ApplyBrake(0f);

            // Constant forward acceleration
            frontLeftWheelCollider.motorTorque = motorForce;
            frontRightWheelCollider.motorTorque = motorForce;
        }
    }

    private void ApplyBrake(float brake)
    {
        frontLeftWheelCollider.brakeTorque = brake;
        frontRightWheelCollider.brakeTorque = brake;
        rearLeftWheelCollider.brakeTorque = brake;
        rearRightWheelCollider.brakeTorque = brake;
    }

    private void HandleSteering()
    {
        float targetSteer = horizontalInput * maxSteerAngle;

        currentSteerAngle = Mathf.Lerp(
            currentSteerAngle,
            targetSteer,
            steeringSpeed * Time.fixedDeltaTime);

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheel(WheelCollider collider, Transform wheel)
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);

        wheel.position = pos;
        wheel.rotation = rot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            platformSpawner.Spawn();
        }
    }
}