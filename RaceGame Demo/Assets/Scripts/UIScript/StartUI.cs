using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public static StartUI Instance { get; private set; }
    private GameObject currentMenu;
    private string selectedCar;
    private string selectedTrack;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        currentMenu = GameObject.Find("StartMenu");
        selectedCar = "Car1";
        selectedTrack = "Track1";
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

    public void SetCarChoice(string carChoice)
    {
        selectedCar = carChoice;
    }

    public void SetTrackChoice(string trackChoice)
    {
        selectedTrack = trackChoice;
    }

}
