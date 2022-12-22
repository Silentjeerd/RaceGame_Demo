using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckpointTimes : MonoBehaviour
{
    public static CheckpointTimes checkpoints { get; private set; }

    GUIStyle textStyle;
    private Dictionary<string, float[]> checkpointList = new Dictionary<string, float[]>();

    float[] times;
    private string timerString;
    private int lapCount;

    // Start is called before the first frame update
    void Start()
    {
        checkpoints = this;
        textStyle = GameManager.Instance.getTextStyle();
        times = GameManager.Instance.getTimes();
        lapCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timerString = timeDisplay(times);
        enableFinishLineCollider();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(100, 5, 100, 25), timerString, textStyle);
        int yoffset = 5;
        foreach (KeyValuePair<string,float[]> kv in checkpointList)
        {
            yoffset += 15;
            GUI.Label(new Rect(5, yoffset, 100, 25), kv.Key + " : " + timeDisplay(kv.Value), textStyle);
        }
    }

    public void UpdateCheckpointTimes(Collider checkpoint)
    {
        float[] checkpointTimes = new float[3] { times[0], times[1], times[2] };
        string checkPointName = "Lap: " + lapCount + " - " + checkpoint.gameObject.name;

        if (!checkpointList.ContainsKey(checkPointName)) checkpointList.Add(checkPointName, checkpointTimes);

        checkpoint.gameObject.SetActive(false);

        if (checkpoint.gameObject.name == "Finish") lapFinish();
    }

    private string timeDisplay(float[] times)
    {
        return String.Format("{0:00}:{1:00}:{2:00}", times[0], times[1], times[2]);
    }

    private void enableFinishLineCollider()
    {
        bool active = false;
        foreach (Transform checkpoint in transform)
        {
            if (checkpoint.gameObject.name != "Finish") active = checkpoint.gameObject.activeSelf;
        }
        if (!active) transform.Find("Finish").gameObject.SetActive(true);
    }

    private void lapFinish()
    {
        lapCount++;
        foreach(Transform checkpoint in transform)
        {
            if (checkpoint.gameObject.name != "Finish") checkpoint.gameObject.SetActive(true);
        }
    }
}
