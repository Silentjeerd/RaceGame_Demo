using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GhostMovement : MonoBehaviour
{

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    [SerializeField] string path;

    private StreamReader reader;
    
    // Use this for initialization
    void Start()
    {
        reader = new StreamReader(path, true);
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        string newpos = reader.ReadLine();
        if(newpos != null)
        {
            char[] seperator = { '/' };
            string[] vectors = newpos.Split(seperator);

            this.transform.position = StringToVector3(vectors[1]);
            this.transform.rotation = StringToQuat(vectors[2]);
            frontLeftTransform.rotation = StringToQuat(vectors[3]);
            frontRightTransform.rotation = StringToQuat(vectors[4]);

        }
    }

    public static string[] StringSplit(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        //string[] sArray = sVector.Split(',');
        return sVector.Split(',');
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // split the items
        string[] sArray = StringSplit(sVector);

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    public static Quaternion StringToQuat(string s)
    {
        // split the items
        //Debug.Log("String: " + s);
        //Quaternion quat = Quaternion.Parse(s);
        string[] sArray = StringSplit(s);
        //float x = float.Parse(sArray[0]);
        //float y = float.Parse(sArray[1]);
        //float z = float.Parse(sArray[2]);
        //float w = float.Parse(sArray[3]);
        //Debug.Log("X:" + x);
        //Debug.Log("Y:" + y);
        //Debug.Log("Z:" + z);
        //Debug.Log("W:" + w);
        Quaternion quat = new Quaternion(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]), float.Parse(sArray[3]));
        //Debug.Log("Quat: " + quat);
        return quat;   
    }
}
