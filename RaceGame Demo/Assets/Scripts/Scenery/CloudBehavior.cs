using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveCloud();
        if (Time.frameCount % GameManager.Instance.rainfallFrameDelay == 0) produceRain();

    }

    private void moveCloud()
    {
        transform.position += GameManager.Instance.getWindDirection() * Time.deltaTime;
    }

    private void produceRain()
    {

        GameObject rain = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Raindrop"));
        rain.transform.position = this.transform.position;
       // transform.position
    }
}
