using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StartUI : MonoBehaviour
{
    public static StartUI Instance { get; private set; }
    private GameObject currentMenu;
    public GameSettings gameSettings;
    
    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        gameSettings = new GameSettings();
        currentMenu = GameObject.Find("StartMenu");

        gameSettings.selectedCar = "Car1";
        gameSettings.selectedTrack = "Track1";
        gameSettings.playerInput = false;
        gameSettings.saveGhost = false;
        gameSettings.ghostFilePath = "Assets/Resources/TxtGhosts/";
        gameSettings.NNTraining = false;


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMenu(GameObject Menu)
    {
        currentMenu.SetActive(false);
        Menu.SetActive(true);
        currentMenu = Menu;
    }

    //public void SetCarChoice(string carChoice)
    //{
    //    gameSettings.selectedCar = carChoice;
    //}

    //public void SetTrackChoice(string trackChoice)
    //{
    //    gameSettings.selectedTrack = trackChoice;
    //}

    public void OnDisable()
    {
        Save();
    }

    public void Save()
    {
        //GameObject.Find("StartMenu");
        //Convert to Json
        //Debug.Log(gameSettings.selectedTrack);
        string jsonData = JsonUtility.ToJson(gameSettings);
        //Debug.Log(jsonData);
        //Save Json string
        PlayerPrefs.SetString("MySettings", jsonData);
        PlayerPrefs.Save();
    }
}
