using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    /// <summary>
    /// Transforms of the wheels of the Car
    /// Used to turn the wheels.
    /// These are set in the UnityEditor.
    /// </summary>
    [SerializeField] public Transform frontRightTransform;
    [SerializeField] public Transform frontLeftTransform;
    [SerializeField] public Transform backRightTransform;
    [SerializeField] public Transform backLeftTransform;

    public string txtFilePath; //Filepath that is used for saving/loading a .txt file.
}
