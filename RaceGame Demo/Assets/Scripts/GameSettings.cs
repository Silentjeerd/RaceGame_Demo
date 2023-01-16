using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameSettings
{
    public string selectedCar;
    public string selectedTrack;

    public string ghostFilePath;
    public string ghostFolder;
    public List<string> ghostFiles = new List<string>();
    public bool saveGhost;
    
    public bool playerInput;

    public bool NNTraining;
    public uint sensorCount;
    public int agentCount;
    public int generations;

}
