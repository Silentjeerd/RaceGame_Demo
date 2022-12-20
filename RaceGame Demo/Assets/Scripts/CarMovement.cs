using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform car;
    private float speed = 10;

    Vector3 rotationRight = new Vector3(0, 60, 0);
    Vector3 rotationLeft = new Vector3(0, -60, 0);

    //Vector3 forward = car.forward;

    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(car.forward * speed * Time.deltaTime);
        }

        if (Input.GetKey("s"))
        {
            transform.Translate(-car.forward * speed * Time.deltaTime);
        }

        if (Input.GetKey("d"))
        {
            //rotation Quaternion
            Quaternion target = Quaternion.Euler(rotationRight * Time.deltaTime);
            
            //transform.rotation = Quaternion.Euler(transform.rotation, target);
            //transform.Rotate(car.rotation * target);
            //car.MoveRotation(car.rotation * target);
            rb.MoveRotation(rb.rotation * target);
        }

        if (Input.GetKey("a"))
        {
            Quaternion target = Quaternion.Euler(rotationLeft * Time.deltaTime);
            //transform.Rotate(car.rotation * target);
            rb.MoveRotation(rb.rotation * target);
        }
    }

}
