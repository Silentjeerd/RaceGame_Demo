using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    List<string> perspectives = new List<string> { "FirstPerson", "ThirdPerson", "TopDown" };
    public GameObject target; //The car that is being followed.
    public int angleIndex = 0; //Index to itterate through the list of perspectives.


    /// <summary>
    /// LateUpdate is called once at the end of a frame
    /// Calls changePerspective to adjust the camera.
    /// </summary>
    void LateUpdate()
    {
        if (target != null) changePerspective(perspectives[angleIndex]);// cameraAngles[angleIndex]();
    }

    /// <summary>
    /// Itterates to the next angle, resets to index 0 based on list length.
    /// </summary>
    public void nextCameraAngle()
    {
        angleIndex++;
        if (angleIndex >= perspectives.Count) angleIndex = 0;
    }

    /// <summary>
    /// Changes the cameras position and rotation.
    /// Finds/uses the Transform thats attached to the current car.
    /// </summary>
    /// <param name="perspective"></param> The name of the Transform that is being used.
    void changePerspective(string perspective) 
    {
        Transform targetTransform = target.transform.Find("CameraPositions/" + perspective);
        this.transform.position = targetTransform.position;
        this.transform.rotation = targetTransform.rotation;
    }
}
