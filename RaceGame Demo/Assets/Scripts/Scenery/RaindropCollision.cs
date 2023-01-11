using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropCollision : MonoBehaviour
{
    [SerializeField]
    private MeshFilter raindropMesh;

    private Mesh rainMesh;
    private Mesh[] faceMeshes;
    private Vector3 rainScale;

    // Start is called before the first frame update
    void Start()
    {
        rainScale = new Vector3(0.1f, 0.1f, 0.1f);
        rainMesh = raindropMesh.mesh;
        transform.localScale = rainScale;
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        covertToFaces();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.down * 3f) * Time.deltaTime;
        if (transform.position.y <= 0) Destroy(this.gameObject);
    }

    private void covertToFaces()
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        int randomDrops = Random.Range(10, 40);
        //for (int i = 0; i < randomDrops; i++)
        //{
        //    GameObject explosiveFace = new GameObject();
        //    explosiveFace.transform.position += transform.position;
        //    explosiveFace.transform.localScale = rainScale;
        //    explosiveFace.AddComponent<MeshFilter>().mesh = faceMeshes[i];
        //    explosiveFace.AddComponent<MeshRenderer>();
        //    explosiveFace.GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
        //    explosiveFace.AddComponent<rainDroplet>(); //add movementscript.
        //}
        Destroy(this.gameObject);
    }
}
