using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropBehavior : MonoBehaviour
{
    [SerializeField]
    private MeshFilter raindropMesh; //The mesh of the raindrop. Set in the unity editor.

    private Mesh[] faceMeshes;
    private Vector3 rainScale;

    /// <summary>
    /// Set a new scale to be used for the rain and rain splashes.
    /// makes a function call to fill the faceMeshes array.
    /// </summary>
    void Start()
    {
        rainScale = new Vector3(0.05f, 0.1f, 0.05f);
        transform.localScale = rainScale;
        convertToFaces();
    }

    /// <summary>
    /// Makes the rain fall.
    /// If the GameObject clips through the racetrack it will be sent up into the sky.
    /// </summary>
    void Update()
    {
        transform.position += (Vector3.down * 1f) * Time.deltaTime;
        if (transform.position.y <= 0) transform.position = new Vector3(transform.position.x,25,transform.position.z);
    }

    /// <summary>
    /// Iterates over all the triangles in the mesh and adds them to the faceMeshes array.
    /// These seperate triangles are used to animate rain splash.
    /// </summary>
    private void convertToFaces()
    {
        Mesh rainMesh = raindropMesh.mesh;
        int triangleCount = rainMesh.triangles.Length / 3;
        faceMeshes = new Mesh[triangleCount];

        for (int i = 0; i < triangleCount; i++)
        {
            Vector3[] faceVertices = new Vector3[3] { rainMesh.vertices[rainMesh.triangles[i * 3]], rainMesh.vertices[rainMesh.triangles[i * 3 + 1]], rainMesh.vertices[rainMesh.triangles[i * 3 + 2]] };
            Vector3[] facenormals = new Vector3[3] { rainMesh.normals[rainMesh.triangles[i * 3]], rainMesh.normals[rainMesh.triangles[i * 3 + 1]], rainMesh.normals[rainMesh.triangles[i * 3 + 2]] };
            int[] verticesIndex = new int[3] { 0, 1, 2 };
            Mesh faceMesh = new Mesh();
            faceMesh.vertices = faceVertices;
            faceMesh.triangles = verticesIndex;
            faceMesh.normals = facenormals;
            faceMeshes[i] = faceMesh;
        }
    }

    /// <summary>
    /// This gets called upon collision with another GameObject in the world.
    /// The raindrop gets send back into the sky.
    /// It then initializes a random number of rain splashes to animate the collision.
    /// </summary>
    /// <param name="collision"></param> The object that gets collided with.
    private void OnCollisionEnter(Collision collision)
    {
        transform.position = new Vector3(transform.position.x, 25, transform.position.z);
        int splashCount = Random.Range(2, faceMeshes.Length);
        for (int i = 0; i < splashCount; i++)
        {
            GameObject rainSplash = new GameObject();
            rainSplash.transform.parent = gameObject.transform;
            rainSplash.transform.position += transform.position;
            rainSplash.transform.localScale = rainScale;
            rainSplash.AddComponent<MeshFilter>().mesh = faceMeshes[i];
            rainSplash.AddComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
            rainSplash.AddComponent<RaindropSplash>().collisionNormal = collision.contacts[0].normal; //add movementscript and sets the first normal for a realistic bounce.
        }
    }
}
