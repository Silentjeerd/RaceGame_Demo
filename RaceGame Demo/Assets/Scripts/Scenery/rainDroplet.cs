using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rainDroplet : MonoBehaviour
{
    private Vector3 normalDirection;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        gameObject.AddComponent<Rigidbody>().useGravity = true;
        setFaceNormal();
        gameObject.GetComponent<Rigidbody>().velocity = normalDirection * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Random.rotation;
        if (Time.frameCount % 80 == 0) Destroy(this.gameObject);
        //transform.position += normalDirection *0.001f;
        
    }

    private void setFaceNormal()
    {
        Vector3[] meshNormals = gameObject.GetComponent<MeshFilter>().mesh.normals;
        normalDirection = (Vector3.Cross(meshNormals[1] - meshNormals[0], meshNormals[2] - meshNormals[0])).normalized;
    }
}
