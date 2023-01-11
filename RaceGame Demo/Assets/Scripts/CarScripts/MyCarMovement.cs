using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MyCarMovement : MonoBehaviour
{
    public event System.Action HitWall;

    [SerializeField] Rigidbody carBody;
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    [SerializeField] string ghostSavePath;

    public float maxSpeed = 15f;
    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 30f;
    public float downForce = -1000f;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    private StreamWriter writer;

    private CarController controller;

    /// <summary>
    /// The current velocity of the car.
    /// </summary>
    public float Velocity
    {
        get;
        private set;
    }

    /// <summary>
    /// The current rotation of the car.
    /// </summary>
    public Quaternion Rotation
    {
        get;
        private set;
    }

    private float horizontalInput, verticalInput;
    /// <summary>
    /// The current inputs for turning and engine force in this order.
    /// </summary>
    public float[] CurrentInputs
    {
        get { return new float[] { horizontalInput, verticalInput }; }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CarController>();
        if (controller != null && controller.UseUserInput)
            writer = new StreamWriter(ghostSavePath, false);
    }

    private void FixedUpdate()
    {
        carBody.AddForce(0, downForce, 0);
        if (controller != null && controller.UseUserInput)
            CheckInput();

        ApplyInput();

        if (controller != null && controller.UseUserInput)
            WriteFramePos();
    }

    // Checks for user input
    private void CheckInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space)) currentBreakForce = breakingForce; else currentBreakForce = 0f;
    }

    /// <summary>
    /// Sets the engine and turning input according to the given values.
    /// </summary>
    /// <param name="input">The inputs for turning and engine force in this order.</param>
    public void SetInputs(float[] input)
    {
        horizontalInput = input[0];
        verticalInput = input[1];
    }

    private void ApplyInput()
    {
        currentAcceleration = acceleration * verticalInput;

        carBody.velocity = Vector3.ClampMagnitude(carBody.velocity, maxSpeed);

        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;

        currentTurnAngle = maxTurnAngle * horizontalInput;
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
    }


    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }

    private void WriteFramePos()
    {
        writer.WriteLine(Time.frameCount + "/" +
                        this.transform.position + "/" +
                        this.transform.rotation + "/" +
                        frontLeftTransform.rotation + "/" +
                        frontRightTransform.rotation);
        writer.Flush();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (controller.UseUserInput && (other.gameObject.tag == "Checkpoint" || other.gameObject.tag == "Finish"))
        //{
        //    CheckpointTimes.checkpoints.UpdateCheckpointTimes(other);
        //}
        if (other.gameObject.tag == "Checkpoint" || other.gameObject.tag == "Finish")
        {
            CheckpointTimes.Instance.UpdateCheckpointTimes(other);
            controller.CheckpointCaptured();
        }
        if (HitWall != null && other.gameObject.tag == "RaceTrack")
        {
            HitWall();
        }
    }

    public void Stop()
    {
        carBody.velocity = new Vector3(0f,0f,0f);
    }
}
