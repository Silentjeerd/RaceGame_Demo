using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartUI : MonoBehaviour
{
    public static StartUI Instance { get; private set; }
    public GameSettings gameSettings; //GameSettings class to hold the settings.

    public Stack commandStack = new Stack(); //The stack for the menuswap commands.

    /// <summary>
    /// Sets a targetframerate.
    /// Creates the GameSettings and sets some of them.
    /// </summary>
    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        gameSettings = new GameSettings();

        gameSettings.selectedCar = "Car1";
        gameSettings.selectedTrack = "Track1";

        gameSettings.saveGhost = true;
        gameSettings.ghostFilePath = "Assets/Resources/TxtGhosts/";
    }

    /// <summary>
    /// Converts the gameSettings to json and saves the in unitys PlayerPrefs class.
    /// </summary>
    public void Save()
    {
        string jsonData = JsonUtility.ToJson(gameSettings);
        PlayerPrefs.SetString("MySettings", jsonData);
        PlayerPrefs.Save();
    }
}
