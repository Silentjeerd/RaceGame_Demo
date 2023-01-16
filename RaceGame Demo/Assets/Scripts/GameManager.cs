using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameSettings settings;

    private GUIStyle textStyle = new GUIStyle();
    private CarController prevBest, prevSecondBest;    

    private Vector3 windDirection;
    public int rainfallFrameDelay { get; private set; }

    private List<GameObject> carsInScene = new List<GameObject>();
    public GameObject activeCar { get; private set; }
    private int carindex = -1;
    private GameObject freeCam;
    private GameObject carCam;

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

    // Start is called before the first frame update
    void Start()
    {
        LoadSettings();
        LoadAssets();

        windDirection = new Vector3(2f, 0f, 2f);
        rainfallFrameDelay = 20;

        Application.targetFrameRate = 60;
        if (!settings.playerInput)
        {
            TrackManager.Instance.BestCarChanged += OnBestCarChanged;
            EvolutionManager.Instance.StartEvolution();
        }
        CheckpointTimes.Instance.resetCheckpoints();

    }

    // Update is called once per frame
    void Update()
    {
       
        handlePlayerInput();
    }

    public GUIStyle getTextStyle()
    {
        return this.textStyle;
    }

    private void OnBestCarChanged(CarController bestCar)
    {
        //if (bestCar == null)
        //    Camera.SetTarget(null);
        //else
        //    Camera.SetTarget(bestCar.gameObject);

        //if (UIController != null)
        //    UIController.SetDisplayTarget(bestCar);
    }

    public Vector3 getWindDirection()
    {
        return windDirection;
    }

    private void LoadSettings()
    {
        //Load saved Json
        string jsonData = PlayerPrefs.GetString("MySettings");
        Debug.Log(jsonData);
        //put data in struct.
        settings = JsonUtility.FromJson<GameSettings>(jsonData);
        createNewSavefileFolder(settings.selectedTrack + "/");
    }

    public string getCarPrefab()
    {
        return settings.selectedCar;
    }

    private void LoadAssets()
    {
        GameObject var;
        var = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Cameras"));
        var.name = "Cameras";
        var = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Grassplane"));
        var = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/" + settings.selectedTrack));
        if(!settings.playerInput) var.AddComponent<TrackManager>();
        if (settings.NNTraining) gameObject.AddComponent<EvolutionManager>();
        //var.AddComponent<TrackManager>();
        var = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/" + settings.selectedTrack + "scenery"));
        var = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Skybox"));

        var = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/" + settings.selectedCar));
        //activeCar = carsInScene[0];
        freeCam = GameObject.Find("Cameras/FreeCam");//.SetActive(false);
        freeCam.SetActive(false);
        //carCamScript = GameObject.Find("Cameras/CarCam").GetComponent<CameraFollow>();
        carCam = GameObject.Find("Cameras/CarCam");
        carCam.GetComponent<CameraFollow>().setOffset(var);
        if (settings.playerInput) addCarToList(var); else Destroy(var.gameObject);
        if (settings.ghostFiles.Count > 0) spawnGhosts();
        if (carsInScene.Count != 0) changeCarCamera();

    }

    public void spawnGhosts()
    {
        foreach(string ghostfile in settings.ghostFiles)
        {
            Debug.Log("Making a ghost");
            
            GameObject prefab = (GameObject)Resources.Load("Prefabs/Ghost" + settings.selectedCar);
            GameObject car = Instantiate(prefab);
            car.GetComponent<GhostMovement>().path = settings.ghostFilePath + ghostfile + ".txt";
            car.name = ghostfile;
            addCarToList(car);
        }
        //if (GameManager.Instance.settings.ghostFiles.Count > 0) prefab = (GameObject)Resources.Load("Prefabs/Ghost" + GameManager.Instance.settings.selectedCar);
    }

    public void createNewSavefileFolder(string newFolder)
    {
        settings.ghostFilePath += (newFolder);
        System.IO.Directory.CreateDirectory(settings.ghostFilePath);
    }
    public void changeSaveFileFolder(string folder)
    {
        settings.ghostFolder = folder;
        System.IO.Directory.CreateDirectory(settings.ghostFilePath + settings.ghostFolder);
    }

    private void handlePlayerInput()
    {
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Escape was pressed");
        }
        if (Input.GetKeyDown("c"))
        {   
            if (carCam.activeSelf) changeCarCamera();
        }
        if (Input.GetKeyDown("v"))
        {
            if (carCam.activeSelf) carCam.GetComponent<CameraFollow>().nextCameraAngle();
        }
        if (Input.GetKeyDown("f"))
        {
            //toggles between car and free camera.
            freeCam.SetActive(!freeCam.activeSelf);
            carCam.SetActive(!carCam.activeSelf);
        }
    }

    public void addCarToList(GameObject car)
    {
        carsInScene.Add(car);
        Debug.Log(carsInScene.Count);
    }

    //Sets camera focus to next car in scene.
    public void changeCarCamera()
    {
        //int index = carsInScene.IndexOf(activeCar) + 1;
        //if (index >= carsInScene.Count) index = 0;
        //activeCar = carsInScene[index];
        //carCam.GetComponent<CameraFollow>().target = activeCar;
        carindex++;
        //carindex = carsInScene.IndexOf(carsInScene[carindex]) + 1;
        if (carindex >= carsInScene.Count) carindex = 0;
        activeCar = carsInScene[carindex];
        carCam.GetComponent<CameraFollow>().target = activeCar;
        //if (carsInScene[carindex].activeSelf == false) changeCarCamera();
    }
}
