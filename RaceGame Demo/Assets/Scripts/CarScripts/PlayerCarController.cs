using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class PlayerCarController : Car
{
    public event System.Action HitWall;

    [SerializeField] Rigidbody carBody; //The body of the car on which forces are applied.

    //The colliders of the car, these collide with the raceTrack and makes it able to drive.
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;


    private float maxSpeed = 15f; //maximumspeed of the car.
    private float acceleration = 500f; //Determines how fast the car can accelerate.
    private float breakingForce = 500f; //The value that is applied when the breaks are used.
    private float maxTurnAngle = 30f; //Determines how far the wheels can turn.
    private float downForce = -1000f; //downforce applied to the car so it stays on track.

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    private StreamWriter writer; //writer used to save a Ghost of the Cars.

   private AICarController controller; //Used if the car is controlled by the AI.

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

    /// <summary>
    /// Start is called before the first frameupdate.
    /// If a player is racing it gives the car a unique name.
    /// Deactivates the sensors used for NN training and restarts the car.
    /// </summary>
    void Start()
    {
        if (GameManager.Instance.settings.playerInput)
        {
            this.name = "Player" + System.DateTime.Now.ToBinary();
            GameObject.Find("Sensors").SetActive(false);
            RestartStreamWriter();
        }
    }

    /// <summary>
    /// Restarts the StreamWriter.
    /// Also used by AICarController when that restarts a car.
    /// </summary>
    public void RestartStreamWriter()
    {
        if (GameManager.Instance.settings.saveGhost)
        {
            txtFilePath = GameManager.Instance.settings.ghostFilePath + GameManager.Instance.settings.ghostFolder + gameObject.name + ".txt";
            writer = new StreamWriter(txtFilePath, false);
        }
    }

    /// <summary>
    /// Every FixedUpdate we check for userinput if necessary.
    /// apply input which could be user or AI input.
    /// And write the new position/rotation of the car to its corresponding txt file.
    /// </summary>
    private void FixedUpdate()
    {
        carBody.AddForce(0, downForce, 0); //Adds downforce to the car so it doesn't wobble.
        if (GameManager.Instance.settings.playerInput) CheckInput();
        ApplyInput();
        if (GameManager.Instance.settings.saveGhost && writer != null) WriteFramePos();
    }

    /// <summary>
    /// Checks for userinput. Spacebar for breaking and WASD or the arrowkeys for the two axis.
    /// fills the input variables so they can be applied later.
    /// </summary>
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

    /// <summary>
    /// Applies the inputs that are stored in verticalInput and horizontalInput the the car.
    /// And updates the wheels.
    /// </summary>
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

    /// <summary>
    /// Takes the position and rotation of the wheelcollider that is used to drive the car.
    /// Sets these values for the Transform of the wheel which is the part visible in the game.
    /// </summary>
    /// <param name="col"></param> The wheelCollider of the wheel, this is not a visible part.
    /// <param name="trans"></param> The Transform of the wheel, this is the visible part.
    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }

    /// <summary>
    /// Writes the relevant values needed to make a ghostCar drive to a txtfile. 
    /// </summary>
    private void WriteFramePos()
    {
        writer.WriteLine(Time.frameCount + "/" +
                        this.transform.position + "/" +
                        this.transform.rotation + "/" +
                        frontLeftTransform.rotation + "/" +
                        frontRightTransform.rotation);
        
    }

    /// <summary>
    /// This function gets called when the car collides with another collider.
    /// The checkpoints have boxcolliders so they trigger when you drive through them.
    /// If the car has a AICarController this will get notified that it has captured a checkpoint.
    /// CheckpointTimes will also get notified of the collision so it can update its overview.
    /// </summary>
    /// <param name="other"></param> The object that was collided with.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint" || other.gameObject.tag == "Finish")
        {
            if(controller != null ) controller.CheckpointCaptured();
            if(CheckpointManager.Instance.UpdateCheckpointTimes(other) >= 2) Stop();
        }

        if (HitWall != null && other.gameObject.tag == "RaceTrack")
        {
            HitWall(); //This call occurs when a AI controlled car crashes into the wall.
        }
    }

    /// <summary>
    /// Flushes and closes the streamwriter when the track is done.
    /// </summary>
    public void Stop()
    {
        if (writer != null)
        {
            writer.Flush();
            writer.Dispose();
        }
    }

}
