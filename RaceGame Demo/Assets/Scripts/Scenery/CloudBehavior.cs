using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehavior : MonoBehaviour
{
    /// <summary>
    /// transforms the position of the cloud based on time past and the wind direction.
    /// </summary>
    void Update()
    {
        transform.position += GameManager.Instance.getWindDirection() * Time.deltaTime;
    }

}
