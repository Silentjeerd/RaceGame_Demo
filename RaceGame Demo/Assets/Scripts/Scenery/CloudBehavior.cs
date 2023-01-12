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
        transform.position += GameManager.Instance.getWindDirection() * Time.deltaTime;
        //if (Time.frameCount % GameManager.Instance.rainfallFrameDelay == 0) produceRain();

    }

    private void produceRain()
    {

        GameObject rain = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Quad"));
        rain.transform.position = this.transform.position;
       // transform.position
    }
}
