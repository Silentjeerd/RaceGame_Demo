using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    delegate void CameraAngleFunction();
    List<CameraAngleFunction> cameraAngles = new List<CameraAngleFunction>();
    //private struct cameraSettings
    //{
    //    public cameraSettings(Vector3 offset, CameraAngleFunction function)
    //    {
    //        this.offset = offset;
    //        this.functionName = function;
    //    }

    //    public Vector3 offset { get; }
    //    //public Quaternion rotation { get; }
    //    //public float desiredAngle
    //    public CameraAngleFunction functionName;
    //}
    //List<cameraSettings> cameraAngles = new List<cameraSettings>();

    public GameObject target;
    public int angleIndex;
    private Vector3 offset;
    //private float desiredAngle;
    private Quaternion rotation;

    void Start()
    {
        cameraAngles.Add(firstPerson);
        cameraAngles.Add(thirdPerson);
        cameraAngles.Add(topDown);
        angleIndex = 1;
        
        //cameraAngles.Add(new cameraSettings(target.transform.position, firstPerson));
        //cameraAngles.Add(new cameraSettings(target.transform.position - transform.position, thirdPerson)); //ThirdPerson
        //cameraAngles.Add(new cameraSettings(target.transform.position, topDown));
        //nextCameraAngle();
    }

    public void setOffset(GameObject car)
    {
        offset = car.transform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //desiredAngle = target.transform.eulerAngles.y;
        ////Quaternion rotation = Quaternion.Euler(-8, desiredAngle, 0);
        //transform.position = target.transform.position - (rotation * offset);
        //transform.LookAt(target.transform);
        if(target != null) cameraAngles[angleIndex]();
        //float desiredAngle = target.transform.eulerAngles.y;
    }

    public void nextCameraAngle()
    {
        Debug.Log("Changing angle");
        angleIndex++;
        if (angleIndex >= cameraAngles.Count) angleIndex = 0;
        //rotation = cameraAngles[angleIndex].rotation;
        //offset = cameraAngles[angleIndex].offset;
    }

    void firstPerson()
    {
        //offset = target.transform.position - transform.position;
        float desiredAngle = target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y+1.5f, target.transform.position.z);
        transform.rotation = rotation;
        //Vector3(target.transform.position.x, 20, target.transform.position.z);
        //transform.LookAt(target.transform);
    }

    void thirdPerson()
    {
        //offset = target.transform.position - transform.position;
        float desiredAngle = target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(-8, desiredAngle, 0);
        transform.position = target.transform.position - (rotation * offset);// target.transform.position - transform.position);
        //Vector3(target.transform.position.x, 20, target.transform.position.z);
        transform.LookAt(target.transform);
    }

    void topDown()
    {
        ////offset = target.transform.position - transform.position;
        //float rotationY = ;
        ////Debug.Log(rotationY);
        //Quaternion rotation = Quaternion.Euler(90, target.transform.eulerAngles.y, 0);
        //Debug.Log(rotation);
        //Vector3 newPos = new Vector3(target.transform.position.x, 25, target.transform.position.z);
        transform.position = new Vector3(target.transform.position.x, 25, target.transform.position.z);
        transform.rotation = Quaternion.Euler(90, target.transform.eulerAngles.y, 0);


        //Vector3(target.transform.position.x, 20, target.transform.position.z);
        //transform.LookAt(target.transform);
        //transform.rotation = target.transform.rotation;
    }

}
