using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CarController_vishal : MonoBehaviour
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

    [Header("Spawner")]
    [SerializeField] private PlatformSpawner platformSpawner;

    [Header("UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private float scoreMultiplier;
    [SerializeField] Image blackScreen;


    private float score=0;
    bool gameOver;
    private Rigidbody rb;
    [SerializeField] int gameoverSceneNO;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        GetInput();
        if (Mathf.Abs(transform.rotation.y) > 0.5)
            ResetCar();
        score += rb.linearVelocity.magnitude * scoreMultiplier*Time.deltaTime;
        scoreText.text = Mathf.Ceil(score).ToString();
        if (gameOver)
        {
            blackScreen.color = new Color(0, 0, 0, blackScreen.color.a + 2 * Time.deltaTime);
        }
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
        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;
    }
    void ResetCar()
    {
        rb.linearVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
    private void HandleMotor()
    {
        //float targetTorque = verticalInput * motorForce;
        float targetTorque =  motorForce;

        currentMotorTorque = Mathf.Lerp(
            currentMotorTorque,
            targetTorque,
            accelerationSpeed * Time.fixedDeltaTime);

        frontLeftWheelCollider.motorTorque = currentMotorTorque;
        frontRightWheelCollider.motorTorque = currentMotorTorque;

/*        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBrakes();*/
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            print("Collided");
            platformSpawner.Spawn();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bomb"))
        {
            StartCoroutine(GameOver());
        }
    }
    IEnumerator GameOver()
    {
        gameOver = true;
        PlayerPrefs.SetFloat("Score", score);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(gameoverSceneNO);
    }
}