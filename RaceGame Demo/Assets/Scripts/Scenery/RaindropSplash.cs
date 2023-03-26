using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropSplash : MonoBehaviour
{
    private Vector3 faceNormal;
    public Vector3 collisionNormal;

    /// <summary>
    /// Disables shadows, adds a rigidBody so gravity applies to the GameObject.
    /// Calculates the normal of the gameObject after which it gets propelled bassed on its normal and that of the collisionpoint.
    /// </summary>
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        gameObject.AddComponent<Rigidbody>().useGravity = true;
        setFaceNormal();
        gameObject.GetComponent<Rigidbody>().velocity = (faceNormal + collisionNormal)* 2f;
    }

    /// <summary>
    /// Randomly rotates the GameObject.
    /// Destroys the object when it clips through the floor.
    /// </summary>
    void Update()
    {
        transform.rotation = Random.rotation;
        if(transform.position.y < 0) Destroy(this.gameObject);
    }

    /// <summary>
    /// Converts the normals of the face into one.
    /// </summary>
    private void setFaceNormal()
    {
        Vector3[] meshNormals = gameObject.GetComponent<MeshFilter>().mesh.normals;
        faceNormal = (Vector3.Cross(meshNormals[1] - meshNormals[0], meshNormals[2] - meshNormals[0])).normalized;
    }
}
