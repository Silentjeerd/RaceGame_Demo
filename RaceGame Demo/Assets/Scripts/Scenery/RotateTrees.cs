using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrees : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            if (child.gameObject.tag == "Tree") randomRotate(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void randomRotate(GameObject tree)
    {
        int yRotation = Random.Range(0, 360);
        tree.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
