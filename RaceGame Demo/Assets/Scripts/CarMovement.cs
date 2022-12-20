using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CarMovement : MonoBehaviour
{
    
    [SerializeField] Rigidbody carBody;
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    [SerializeField] string path;

    // Start is called before the first frame update
    public float maxSpeed = 15f;
    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;
    public float downForce = -1000f;


    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    private StreamWriter writer;

    void Start()
    {
        Application.targetFrameRate = 60;
        writer = new StreamWriter(path, false);
        
    }

    private void FixedUpdate()
    {
        carBody.AddForce(0, downForce, 0);

        currentAcceleration = acceleration * Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space)) currentBreakForce = breakingForce; else currentBreakForce = 0f;

        carBody.velocity = Vector3.ClampMagnitude(carBody.velocity, maxSpeed);

        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        //frontRight.brakeTorque = currentBreakForce;
        //frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        WriteFramePos();
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
                        this.transform.rotation);
        //+ "/" + 
        //                frontLeftTransform.rotation + "/" +
        //                frontRightTransform.rotation);
        writer.Flush();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Object that collided with me: " + other.gameObject.name);
        //WriteFramePos();
        //string path = "Assets/Resources/TxtGhosts/Ghost1.txt";
        //Write some text to the test.txt file
        //StreamWriter writer = new StreamWriter(path, true);
        //writer.WriteLine(other.gameObject.name + " passed on frame " + Time.frameCount);
        //writer.Close();
    }


}
