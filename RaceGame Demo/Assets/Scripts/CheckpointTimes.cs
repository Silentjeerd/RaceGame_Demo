using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckpointTimes : MonoBehaviour
{
    public static CheckpointTimes Instance { get; private set; }

    GUIStyle textStyle;
    private Dictionary<string, float[]> checkpointList = new Dictionary<string, float[]>();

    private float[] displayTimer = new float[3];
    private float timer;
    private string timerString;

    private int lapCount;
    private Checkpoint[] checkpointsArray;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        textStyle = GameManager.Instance.getTextStyle();
        lapCount = 1;
        checkpointsArray = GetComponentsInChildren<Checkpoint>();
    }

    // Update is called once per frame
    // Updates the time that has passed.
    void Update()
    {
        timer += Time.deltaTime;
        displayTimer[0] = Mathf.FloorToInt(timer / 60);
        displayTimer[1] = Mathf.FloorToInt(timer % 60);
        displayTimer[2] = (timer * 1000) % 1000;
    }

    /// <summary>
    /// Draws the racetime and checkpointtimes on the GUI.
    /// </summary>
    void OnGUI()
    {
        GUI.Label(new Rect(100, 5, 100, 25), timeDisplay(displayTimer), textStyle);
        int yoffset = 5;
        foreach (KeyValuePair<string,float[]> kv in checkpointList)
        {
            yoffset += 15;
            GUI.Label(new Rect(5, yoffset, 100, 25), kv.Key + " : " + timeDisplay(kv.Value), textStyle);
        }
    }

    /// <summary>
    /// This function is called when a car collides with a checkpoint.
    /// It will put the name and time into a Dictionary.
    /// </summary>
    /// <param name="checkpoint"></param> The checkpoint that was collided with.
    public void UpdateCheckpointTimes(Collider checkpoint)
    {
        float[] checkpointTimes = new float[3] { displayTimer[0], displayTimer[1], displayTimer[2] };
        string checkPointName = "Lap: " + lapCount + " - " + checkpoint.gameObject.name;

        if (!checkpointList.ContainsKey(checkPointName)) checkpointList.Add(checkPointName, checkpointTimes);

        if (checkpoint.gameObject.name == "Finish")
        {
            lapCount++;
            resetCheckpoints();
        }
        else
        {
            checkpointCollision(checkpoint);
        }
    }

    /// <summary>
    /// Converts a float array containing time information to a string.
    /// </summary>
    /// <param name="times"></param> The array to be converted.
    /// <returns></returns> The converted array as a string.
    private string timeDisplay(float[] times)
    {
        return String.Format("{0:00}:{1:00}:{2:00}", times[0], times[1], times[2]);
    }

    /// <summary>
    /// Resets all checkpoints and the racetimer.
    /// </summary>
    public void resetCheckpoints()
    {
        checkpointsArray[0].gameObject.SetActive(true);
        for (int i = 1; i < checkpointsArray.Length; i++)
        {
            checkpointsArray[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Checks with which checkpoint a collision occured.
    /// Deactivates said checkpoint and activates the next.
    /// </summary>
    /// <param name="checkpoint"></param> The checkpoint that was collided with.
    private void checkpointCollision(Collider checkpoint)
    {
        for (int i = 0; i < checkpointsArray.Length; i++)
        {
            if (checkpointsArray[i].gameObject.name == checkpoint.gameObject.name)
            {
                checkpoint.gameObject.SetActive(false);
                checkpointsArray[i + 1].gameObject.SetActive(true);
            }
        }
    }

    public void reset()
    {
        timer = 0f;
        lapCount = 1;
        checkpointList.Clear();
        resetCheckpoints();
    }
}
