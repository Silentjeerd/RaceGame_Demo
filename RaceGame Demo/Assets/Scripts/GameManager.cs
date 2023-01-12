using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GUIStyle textStyle = new GUIStyle();
    private CarController prevBest, prevSecondBest;
    public bool UseUserInput = false;

    private Vector3 windDirection;
    public int rainfallFrameDelay { get; private set; }

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
        windDirection = new Vector3(2f, 0f, 2f);
        rainfallFrameDelay = 20;
        Application.targetFrameRate = 60;
        if (!UseUserInput)
        {
            TrackManager.Instance.BestCarChanged += OnBestCarChanged;
            EvolutionManager.Instance.StartEvolution();
        }
        CheckpointTimes.Instance.resetCheckpoints();
    }

    // Update is called once per frame
    void Update()
    {

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

}
