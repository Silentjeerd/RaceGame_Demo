using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GUIStyle textStyle = new GUIStyle();
    private float[] displayTimes = new float[3];
    //private List<KeyValuePair<string, float>> checkpointList = new List<KeyValuePair<string, float>>();

    private void Awake()
    {
        Instance = this;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //checkpointTimes = new List<KeyValuePair<string, float>>();
    }

    // Update is called once per frame
    void Update()
    {
        displayTimes[0] = Mathf.FloorToInt(Time.time / 60);
        displayTimes[1] = Mathf.FloorToInt(Time.time % 60);
        displayTimes[2] = (Time.time * 1000) % 1000;
    }

    //public void addCheckpointTime(string checkpointName, float checkpointTime)
    //{
    //    checkpointList.Add(new KeyValuePair<string, float>(checkpointName, checkpointTime));
    //    CheckpointTimes.UpdateCheckpointTimes();
    //}

    //public List<KeyValuePair<string,float>> getCheckpointList()
    //{
    //    return this.checkpointList;
    //}

    public GUIStyle getTextStyle()
    {
        return this.textStyle;
    }

    public float[] getTimes()
    {
        return this.displayTimes;
    }
}
