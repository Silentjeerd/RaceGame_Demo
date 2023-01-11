using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public float CaptureRadius = 3;
    /// <summary>
    /// The reward value earned by capturing this checkpoint.
    /// </summary>
    public float RewardValue
    {
        get;
        set;
    }

    public float DistanceToPrevious
    {
        get;
        set;
    }

    public float AccumulatedDistance
    {
        get;
        set;
    }

    public float AccumulatedReward
    {
        get;
        set;
    }

    void Awake()
    {
        CaptureRadius = 0;
    }

    public float GetRewardValue(float currentDistance)
    {
        //Calculate how close the distance is to capturing this checkpoint, relative to the distance from the previous checkpoint
        float completePerc = (DistanceToPrevious - currentDistance) / DistanceToPrevious;

        //Reward according to capture percentage
        if (completePerc < 0)
            return 0;
        else return completePerc * RewardValue;
    }
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
