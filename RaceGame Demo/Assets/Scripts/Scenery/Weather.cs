using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    private float north = 102f; //z most northern point of gamezone.
    private float east = 25f; //x most eastern point of gamezone.
    private float south = -93f; //z most southern point of gamezone.
    private float west = -175f; //x most western point of gamezone.
    private float rainRadius = 5f;//Offset for rain around current car.

    private Vector3 windDir;

    private List<GameObject> clouds = new List<GameObject>(); //List that holds all cloud objects.
    private List<GameObject> rainDrops = new List<GameObject>(); //List that holds all rain objects.

    [SerializeField]
    public Mesh[] cloudMeshes; //Array of different cloud meshes.


    // Start is called before the first frame update
    void Start()
    {
        instantiateWeather();
    }

    // Gets called every frame.
    void Update()
    {
        cloudOutOfBounds();
        rainDropOutOfBounds();
    }

    /// <summary>
    /// Generates a random spawn position within the bounds of the gamezone.
    /// </summary>
    /// <returns></returns> A Vector3 holding the spawn coördinates.
    private Vector3 spawnPos()
    {
        float x = Random.Range(west, east);
        float z = Random.Range(south, north);
        float y = Random.Range(22, 25);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// This generates the weather.
    /// It randomizes a value for how cloudy it will be and based on that generates the clouds and raindrops.
    /// </summary>
    private void instantiateWeather()
    {
        int cloudiness = Random.Range(0, 5);
        int maxClouds = cloudiness * 100;
        int maxRaindrops = cloudiness * 10;

        for (int i = 0; i < maxClouds; i++)
        {
            GameObject cloud = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Environment/Cloud"));
            cloud.transform.parent = gameObject.transform;
            cloud.GetComponent<MeshFilter>().mesh = cloudMeshes[Random.Range(0, cloudMeshes.Length)];
            cloud.transform.position = spawnPos();
            cloud.transform.rotation = Random.rotation;
            clouds.Add(cloud);

        }

        for (int i = 0; i < maxRaindrops; i++)
        {
            GameObject rain = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Environment/SimpleRain"));
            rain.transform.parent = gameObject.transform;
            rain.transform.position = spawnPos();
            rainDrops.Add(rain);
        }
    }

    /// <summary>
    /// Iterates over every cloud in the list and checks if it is flying out of bounds.
    /// When it hits a border its position will get adjusted.
    /// </summary>
    private void cloudOutOfBounds()
    {
        foreach(GameObject cloud in clouds)
        {
            Vector3 pos = cloud.transform.position;
            if (cloud.transform.position.x > east) pos.x = west;
            if (cloud.transform.position.x < west) pos.x = east;
            if (cloud.transform.position.z > north) pos.z = south;
            if (cloud.transform.position.z < south) pos.z = north;
            cloud.transform.position = pos;
        }
    }

    /// <summary>
    /// Iterates over every raindrop in the list and checks if it is within its bounds.
    /// This is based on the position of the car.
    /// </summary>
    private void rainDropOutOfBounds()
    {
        if (GameManager.Instance.activeCar != null)
        {
            Vector3 bounds = GameManager.Instance.activeCar.transform.position;
            foreach (GameObject rain in rainDrops)
            {
                Vector3 newRainPos = rain.transform.position;
                if (rain.transform.position.x > bounds.x + rainRadius) newRainPos.x = bounds.x - rainRadius;
                if (rain.transform.position.x < bounds.x - rainRadius) newRainPos.x = bounds.x + rainRadius;
                if (rain.transform.position.z > bounds.z + rainRadius) newRainPos.z = bounds.z - rainRadius;
                if (rain.transform.position.z < bounds.z - rainRadius) newRainPos.z = bounds.z + rainRadius;
                rain.transform.position = newRainPos;
            }
        }
    }
}
