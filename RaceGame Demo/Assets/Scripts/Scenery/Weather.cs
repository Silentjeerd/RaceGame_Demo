using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    private float north = 102f; //z
    private float east = 25f; //x
    private float south = -93f; //z
    private float west = -175f; //x

    private int cloudiness;
    private int maxClouds;
    private Vector3 windDir;

    private List<GameObject> clouds = new List<GameObject>();
    
    [SerializeField]
    public Mesh[] cloudMeshes;

    [SerializeField]
    public BoxCollider box;

    // Start is called before the first frame update
    void Start()
    {
        cloudiness = Random.Range(0, 10);
        maxClouds = 100 * cloudiness;
        //maxClouds = 1000;
        for(int i = 0; i < maxClouds; i++)
        {
            GameObject cloud = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Cloud"));
            cloud.GetComponent<MeshFilter>().mesh = cloudMeshes[Random.Range(0, cloudMeshes.Length)];
            cloud.transform.position = spawnPos();
            cloud.transform.rotation = Random.rotation;
            clouds.Add(cloud);
            //GameObject rain = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Quad"));
            //rain.transform.position = spawnPos();
            //clouds.Add(rain);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if (clouds.Count < maxClouds)
        //{
        //    GameObject cloud = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Cloud"));
        //    cloud.GetComponent<MeshFilter>().mesh = cloudMeshes[Random.Range(0, cloudMeshes.Length)];
        //    cloud.transform.position = spawnPos();
        //    cloud.transform.rotation = Random.rotation;
        //    clouds.Add(cloud);
        //}
        cloudOutofBounds();
    }

    private Vector3 spawnPos()
    {
        float x = Random.Range(west, east);
        float z = Random.Range(south, north);
        float y = Random.Range(22, 25);
        //cloudPos.x = x;
        //cloudPos.y = y;
        //cloudPos.z = z;
        return new Vector3(x, y, z);
    }

    private void cloudOutofBounds()
    {
        foreach(GameObject cloud in clouds)
        {
            Vector3 pos = cloud.transform.position;
            if (cloud.transform.position.x > east) pos.x = west;
            if (cloud.transform.position.x < west) pos.x = east;
            if (cloud.transform.position.z > north) pos.z = south;
            if (cloud.transform.position.z < south) pos.z = north;
            cloud.transform.position = pos;
            //if (cloud.transform.position.y < 0) cloud.transform.position = spawnPos();
        }
    }
}
