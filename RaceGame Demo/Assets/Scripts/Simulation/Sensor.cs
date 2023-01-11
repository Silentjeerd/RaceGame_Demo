/// Author: Samuel Arzt
/// Date: March 2017

#region Includes
using UnityEngine;
#endregion

/// <summary>
/// Class representing a sensor reading the distance to the nearest obstacle in a specified direction.
/// </summary>
public class Sensor : MonoBehaviour
{
    #region Members
    // The layer this sensor will be reacting to, to be set in Unity editor.
    [SerializeField]
    private LayerMask LayerToSense;
    //The crosshair of the sensor, to be set in Unity editor.
    [SerializeField]
    private SpriteRenderer Cross;

    // Max and min readings
    private const float MAX_DIST = 20f;
    private const float MIN_DIST = 0.01f;

    /// <summary>
    /// The current sensor readings in percent of maximum distance.
    /// </summary>
    public float Output
    {
        get;

        private set;
    }
    #endregion

    #region Constructors
    void Start ()
    {
       Cross.gameObject.SetActive(true);
	}
    #endregion

    #region Methods
    // Unity method for updating the simulation
    void FixedUpdate ()
    {
        RayCastTest();
	}

    void RayCastTest()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        //Vector3 pos = transform.position;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MAX_DIST, LayerToSense))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); 
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * MAX_DIST, Color.yellow);
        }

        //Check distance
        if (hit.collider == null)
            hit.distance = MAX_DIST;
        else if (hit.distance < MIN_DIST)
            hit.distance = MIN_DIST;

        Cross.transform.position = hit.point;
        this.Output = hit.distance; //transform to percent of max distance
    }

    /// <summary>
    /// Hides the crosshair of this sensor.
    /// </summary>
    public void Hide()
    {
       Cross.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the crosshair of this sensor.
    /// </summary>
    public void Show()
    {
       Cross.gameObject.SetActive(true);
    }
    #endregion
}
