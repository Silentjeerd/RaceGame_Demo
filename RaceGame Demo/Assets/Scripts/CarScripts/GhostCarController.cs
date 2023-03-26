using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GhostCarController : Car
{

    private StreamReader reader;
    
    /// <summary>
    /// Initializes the StreamReader upon creation.
    /// txtFilePath is stored in Car.
    /// </summary>
    void Start()
    {
        reader = new StreamReader(txtFilePath, true);
    }


    /// <summary>
    /// Reads and stores the next line from the txt file.
    /// Seperates the data into a string array.
    /// Sets all the new values for the GhostCar.
    /// </summary>
    private void FixedUpdate()
    {
        string nextPosition = reader.ReadLine();
        if(nextPosition != null)
        {
            char[] seperator = { '/' };
            string[] transformationData = nextPosition.Split(seperator);

            this.transform.position = StringToVector3(transformationData[1]);
            this.transform.rotation = StringToQuaternion(transformationData[2]);
            frontLeftTransform.rotation = StringToQuaternion(transformationData[3]);
            frontRightTransform.rotation = StringToQuaternion(transformationData[4]);

        }
    }

    /// <summary>
    /// Takes a string of values that have to be split into an array.
    /// </summary>
    /// <param name="s"></param> The string that has to be split.
    /// <returns></returns> The string split into an array.
    public static string[] StringSplit(string s)
    {
        // Remove the parentheses
        if (s.StartsWith("(") && s.EndsWith(")"))
        {
            s = s.Substring(1, s.Length - 2);
        }

        // split the items
        return s.Split(',');
    }

    /// <summary>
    /// Converts a string to a vector3 that holds the new position of the GhostCar.
    /// </summary>
    /// <param name="vectorString"></param> string that holds the data for the Vector.
    /// <returns></returns> A vector3 that holds the new position of the GhostCar.
    public static Vector3 StringToVector3(string vectorString)
    {
        // split the items
        string[] sArray = StringSplit(vectorString);

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    /// <summary>
    /// Converts a string to a Quaternion that holds a rotation for a GameObject.
    /// </summary>
    /// <param name="quaternionString"></param> The string that holds the data for the Quaternion
    /// <returns></returns> A Quaternion that holds the new Rotation of a GameObject.
    public static Quaternion StringToQuaternion(string quaternionString)
    {
        string[] sArray = StringSplit(quaternionString);
        Quaternion quaternion = new Quaternion(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]), float.Parse(sArray[3]));
        return quaternion;   
    }
}
