using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameSettings settings; //This object will get filled by loadsettings, after which the other scripts can access these.

    private GUIStyle textStyle = new GUIStyle(); 

    private GameObject carSpawnPoint; //Spawnpoint of the cars.
    private List<GameObject> carsInScene = new List<GameObject>(); //List of all the cars in the scene.
    public GameObject activeCar { get; private set; } //The currently focused car.
    private int carindex = -1;
    
    private GameObject freeCamera; //Camera with which you can move through the scene freely.
    private GameObject carCamera; //Camera that is attached to a car.

    private GameObject pauseMenu; //Pausemenu object to be set so the game can be paused.
    private GameObject weather; //Gameobject to be set so it can be toggled on/off
    private Vector3 windDirection; //Winddirection which will be used by the weather.

    delegate void keyBind();
    private List<KeyValuePair<string, keyBind>> keybinds = new List<KeyValuePair<string, keyBind>>();

    /// <summary>
    /// Makes sure it is the only instance of this class.
    /// Sets some textStyle variables.
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameStateManagers in the Scene.");
            return;
        }
        Instance = this;

        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    /// <summary>
    /// This function gets called first after the scene is loaded.
    /// </summary>
    void Start()
    {
        LoadSettings(); //Loads the settings from the startmenu.
        LoadAssets(); //Initializes the assets.
        setKeybinds(); //Sets the keybinds.

        windDirection = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)); //Sets a random winddirection for the clouds.
        Application.targetFrameRate = 60; //Sets the targeted framerate to 60.
        if (!settings.playerInput) EvolutionManager.Instance.StartEvolution(); //Start the training of the neural network if there is no player.
        CheckpointManager.Instance.resetCheckpoints(); //Resets the checkpoints/timers.
    }

    /// <summary>
    /// This is called every frame.
    /// </summary>
    void Update()
    {
        handlePlayerInput();
    }

    /// <summary>
    /// function to get the textstyle that has to be used in the UI.
    /// </summary>
    /// <returns></returns> The textStyle to be used.
    public GUIStyle getTextStyle()
    {
        return this.textStyle;
    }

    /// <summary>
    /// function used by clouds to get the winddirection.
    /// </summary>
    /// <returns></returns>
    public Vector3 getWindDirection()
    {
        return windDirection;
    }

    /// <summary>
    /// loads the settings which were set in the Startmenu. 
    /// These settings are stored in the PlayerPrefs in json format.
    /// </summary>
    private void LoadSettings()
    {
        //Load saved Json
        string jsonData = PlayerPrefs.GetString("MySettings");
        //put data in struct.
        settings = JsonUtility.FromJson<GameSettings>(jsonData);
        createNewSavefileFolder(settings.selectedTrack + "/");
    }

    /// <summary>
    /// Loads all the assets into the scene.
    /// </summary>
    private void LoadAssets()
    {
        pauseMenu = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/UI/pauseMenu"));
        pauseMenu.SetActive(false);
        pauseMenu.transform.parent = gameObject.transform;
        
        GameObject placeHolderObject;

        //loads the Cameras from a prefab and sets their respective GameObject variables.
        placeHolderObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Cameras/Cameras"));
        placeHolderObject.name = "Cameras";
        freeCamera = GameObject.Find("Cameras/FreeCam");
        freeCamera.SetActive(false);
        carCamera = GameObject.Find("Cameras/CarCam");

        placeHolderObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Environment/Grassplane")); //loads the grassplane
        placeHolderObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Tracks/" + settings.selectedTrack)); //loads the track.
        carSpawnPoint = GameObject.Find("Spawn"); //sets the spawnpoint of the cars.

        //adds the TrackManager and EvolutionManager scripts if AI-Training has been selected.
        if (settings.NNTraining)
        {
            placeHolderObject.AddComponent<TrackManager>();
            gameObject.AddComponent<EvolutionManager>();
        }

        //loads the scenery and skybox.
        placeHolderObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Environment/" + settings.selectedTrack + "scenery"));
        placeHolderObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Environment/Skybox"));
        weather = GameObject.Find("Clouds"); //sets the weather variable so this can be toggled on/off.

        //if the player is racing himself this loads/spawns his car and adds it to the list.
        if (settings.playerInput)
        {
            placeHolderObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Cars/" + settings.selectedCar));
            placeHolderObject.transform.position = carSpawnPoint.transform.position;
            placeHolderObject.transform.rotation = carSpawnPoint.transform.rotation;
            addCarToList(placeHolderObject);
        }

        if (settings.ghostFiles.Count > 0) spawnGhosts(); //If ghosts have been selected they get spawned.
        if (carsInScene.Count != 0) changeCarCamera(); //changes the focus of the camera.

    }

    /// <summary>
    /// Spawns a ghostcar for each of the selected ghostfiles.
    /// </summary>
    public void spawnGhosts()
    {
        foreach(string ghostfile in settings.ghostFiles)
        {
            GameObject ghostCar = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Cars/Ghost" + settings.selectedCar));
            ghostCar.GetComponent<GhostCarController>().txtFilePath = settings.ghostFilePath + ghostfile + ".txt";
            ghostCar.name = ghostfile;
            ghostCar.transform.position = carSpawnPoint.transform.position;
            ghostCar.transform.rotation = carSpawnPoint.transform.rotation;
            addCarToList(ghostCar);
        }
    }

    /// <summary>
    /// Will create a new folder if it doesnt already exist.
    /// </summary>
    /// <param name="newFolder"></param> the folder to be created.
    public void createNewSavefileFolder(string newFolder)
    {
        settings.ghostFilePath += newFolder;
        System.IO.Directory.CreateDirectory(settings.ghostFilePath);
    }

    /// <summary>
    /// Will create a new folder if it doesnt already exist.
    /// Not sure anymore why there are two of these functions.
    /// </summary>
    /// <param name="folder"></param> the folder to be created.
    public void changeSaveFileFolder(string folder)
    {
        settings.ghostFolder = folder;
        System.IO.Directory.CreateDirectory(settings.ghostFilePath + settings.ghostFolder);
    }

    /// <summary>
    /// Function to add cars to the list of cars in the scene.
    /// This function is also used by the TrackManager when it generates the AIcars.
    /// </summary>
    /// <param name="car"></param> The car that has to be added to the list.
    public void addCarToList(GameObject car)
    {
        carsInScene.Add(car);
    }

    /// <summary>
    /// Iterates over all keybinds to see which has been pressed, if any.
    /// </summary>
    private void handlePlayerInput()
    {
        foreach (KeyValuePair<string, keyBind> kv in keybinds)
        {
            if (Input.GetKeyDown(kv.Key)) kv.Value(); //If the pressed key has a keybind this gets called here.
        }
    }

    /// <summary>
    /// Fills the keybinds list with keybindings, hardcoded for the timebeing.
    /// </summary>
    private void setKeybinds()
    {
        keybinds.Add(new KeyValuePair<string, keyBind>("escape", pauseGame));
        keybinds.Add(new KeyValuePair<string, keyBind>("c", changeCarCamera));
        keybinds.Add(new KeyValuePair<string, keyBind>("g", toggleWeather));
        keybinds.Add(new KeyValuePair<string, keyBind>("f", switchCameraStyle));
        keybinds.Add(new KeyValuePair<string, keyBind>("v", changeCameraPerspective));
    }

    /// <summary>
    /// Focuses the next available car in the scene.
    /// </summary>
    public void changeCarCamera()
    {
        if (carCamera.activeSelf)
        {
            carindex++;
            if (carindex >= carsInScene.Count) carindex = 0;
            activeCar = carsInScene[carindex];
            carCamera.GetComponent<CameraFollow>().target = activeCar;
        }
    }

    /// <summary>
    /// Toggle to switch between firstperson, thirdperson or topdown view.
    /// </summary>
    private void changeCameraPerspective()
    {
        if (carCamera.activeSelf) carCamera.GetComponent<CameraFollow>().nextCameraAngle();
    }

    /// <summary>
    /// Toggles the weather gameobject. Enables/disables the rain and clouds.
    /// </summary>
    private void toggleWeather()
    {
        if (!weather.activeSelf) weather.SetActive(true); else weather.SetActive(false);
    }

    /// <summary>
    /// Toggles between the car focused camera and the freecamera.
    /// </summary>
    private void switchCameraStyle()
    {
        freeCamera.SetActive(!freeCamera.activeSelf);
        carCamera.SetActive(!carCamera.activeSelf);
    }

    /// <summary>
    /// Toggles between pausing/unpausing the game.
    /// Shows a menu and freezes the time.
    /// </summary>
    private void pauseGame()
    {
        if (!pauseMenu.activeSelf) { Time.timeScale = 0; pauseMenu.SetActive(true); } else { Time.timeScale = 1; pauseMenu.SetActive(false); }
    }
}
