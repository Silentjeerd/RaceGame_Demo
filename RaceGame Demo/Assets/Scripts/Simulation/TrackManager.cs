/// Author: Samuel Arzt
/// Date: March 2017

#region Includes
using System;
using UnityEngine;
using System.Collections.Generic;
#endregion

/// <summary>
/// Singleton class managing the current track and all cars racing on it, evaluating each individual.
/// </summary>
public class TrackManager : MonoBehaviour
{

    public static TrackManager Instance
    {
        get;
        private set;
    }

    // Sprites for visualising best and second best cars. To be set in Unity Editor.
    [SerializeField]
    private Sprite BestCarSprite;
    [SerializeField]
    private Sprite SecondBestSprite;
    [SerializeField]
    private Sprite NormalCarSprite;

    private Checkpoint[] checkpoints;

    private int currentCarIndex;

    private GameObject carPrefab; // Car used to create new cars and to set start position.
    private GameObject carSpawnpoint; // Start position for cars

    // Struct for storing the current cars and their position on the track.
    private class RaceCar
    {
        public RaceCar(AICarController car = null, uint checkpointIndex = 0)
        {
            this.Car = car;
            this.CheckpointIndex = checkpointIndex;
        }
        public AICarController Car;
        public uint CheckpointIndex;
    }
    private List<RaceCar> cars = new List<RaceCar>();

    /// <summary>
    /// The amount of cars currently on the track.
    /// </summary>
    public int CarCount
    {
        get { return cars.Count; }
    }

    private AICarController bestCar = null;
    /// <summary>
    /// The current best car (furthest in the track).
    /// </summary>
    public AICarController BestCar
    {
        get { return bestCar; }
        private set
        {
            if (bestCar != value)
            {
                //Update appearance
                if (BestCar != null)
                    BestCar.SpriteRenderer.sprite = NormalCarSprite;
                if (value != null)
                    value.SpriteRenderer.sprite = BestCarSprite;

                //Set previous best to be second best now
                AICarController previousBest = bestCar;
                bestCar = value;
                if (BestCarChanged != null)
                    BestCarChanged(bestCar);

                SecondBestCar = previousBest;
            }
        }
    }
    /// <summary>
    /// Event for when the best car has changed.
    /// </summary>
    public event System.Action<AICarController> BestCarChanged;

    private AICarController secondBestCar = null;
    /// <summary>
    /// The current second best car (furthest in the track).
    /// </summary>
    public AICarController SecondBestCar
    {
        get { return secondBestCar; }
        private set
        {
            if (SecondBestCar != value)
            {
                //Update appearance of car
                if (SecondBestCar != null && SecondBestCar != BestCar)
                    SecondBestCar.SpriteRenderer.sprite = NormalCarSprite;
                if (value != null)
                    value.SpriteRenderer.sprite = SecondBestSprite;

                secondBestCar = value;
                if (SecondBestCarChanged != null)
                    SecondBestCarChanged(SecondBestCar);
            }
        }
    }
    /// <summary>
    /// Event for when the second best car has changed.
    /// </summary>
    public event System.Action<AICarController> SecondBestCarChanged;

    /// <summary>
    /// The length of the current track in Unity units (accumulated distance between successive checkpoints).
    /// </summary>
    public float TrackLength
    {
        get;
        private set;
    }


    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of TrackManager are not allowed in one Scene.");
            return;
        }

        Instance = this;
        carSpawnpoint = GameObject.Find("Spawn");//sets the car spawnpoint.
        //Get all checkpoints
        checkpoints = GetComponentsInChildren<Checkpoint>();
        carPrefab = (GameObject)Resources.Load("Prefabs/Cars/" + GameManager.Instance.settings.selectedCar); //loads the car prefab.

        CalculateCheckpointPercentages();
    }


    // Unity method for updating the simulation
    void Update()
    {
        if (currentCarIndex < cars.Count)
        {
            RaceCar car = cars[currentCarIndex];
            if (!car.Car.Agent.IsAlive)
            {
                currentCarIndex++;
                CheckpointManager.Instance.reset();
                cars[currentCarIndex].Car.Restart();
                GameManager.Instance.changeCarCamera();
            }
            if (car.Car.enabled)
            {
                car.Car.CurrentCompletionReward = GetCompletePerc(car.Car, ref car.CheckpointIndex);
                
                //Update best
                if (BestCar == null || car.Car.CurrentCompletionReward >= BestCar.CurrentCompletionReward)
                    BestCar = car.Car;
                else if (SecondBestCar == null || car.Car.CurrentCompletionReward >= SecondBestCar.CurrentCompletionReward)
                    SecondBestCar = car.Car;
            }
        }

    }

    public void SetCarAmount(int amount)
    {
        //Check arguments
        if (amount < 0) throw new ArgumentException("Amount may not be less than zero.");

        if (amount == CarCount) return;

        if (amount > cars.Count)
        {
            //Add new cars
            for (int toBeAdded = amount - cars.Count; toBeAdded > 0; toBeAdded--)
            {
                GameObject AICar = Instantiate(carPrefab,carSpawnpoint.transform);
                AICarController AIController = AICar.AddComponent<AICarController>(); 
                cars.Add(new RaceCar(AIController, 0));
                AICar.SetActive(false);
                GameManager.Instance.addCarToList(AICar);
            }
        }
        else if (amount < cars.Count)
        {
            //Remove existing cars
            for (int toBeRemoved = cars.Count - amount; toBeRemoved > 0; toBeRemoved--)
            {
                RaceCar last = cars[cars.Count - 1];
                cars.RemoveAt(cars.Count - 1);

                Destroy(last.Car.gameObject);
            }
        }
    }

    /// <summary>
    /// Restarts all cars and puts them at the track start.
    /// </summary>
    public void Restart()
    {
        foreach (RaceCar car in cars)
        {
            car.Car.transform.position = carSpawnpoint.transform.position;
            car.Car.transform.rotation = carSpawnpoint.transform.rotation;
            car.CheckpointIndex = 0;
        }

        currentCarIndex = 0;
        BestCar = null;
        SecondBestCar = null;
        CheckpointManager.Instance.resetCheckpoints();
        cars[currentCarIndex].Car.Restart();
        GameManager.Instance.changeCarCamera();
    }

    /// <summary>
    /// Returns an Enumerator for iterator through all cars currently on the track.
    /// </summary>
    public IEnumerator<AICarController> GetCarEnumerator()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            yield return cars[i].Car;
        }

    }

    /// <summary>
    /// Calculates the percentage of the complete track a checkpoint accounts for. This method will
    /// also refresh the <see cref="TrackLength"/> property.
    /// </summary>
    private void CalculateCheckpointPercentages()
    {

        checkpoints[0].AccumulatedDistance = 0; //First checkpoint is start

        //Iterate over remaining checkpoints and set distance to previous and accumulated track distance.
        for (int i = 1; i < checkpoints.Length; i++)
        {
            checkpoints[i].DistanceToPrevious = Vector3.Distance(checkpoints[i].transform.position, checkpoints[i - 1].transform.position);
            checkpoints[i].AccumulatedDistance = checkpoints[i - 1].AccumulatedDistance + checkpoints[i].DistanceToPrevious;
        }

        //Set track length to accumulated distance of last checkpoint
        TrackLength = checkpoints[checkpoints.Length - 1].AccumulatedDistance;
        
        //Calculate reward value for each checkpoint
        for (int i = 1; i < checkpoints.Length; i++)
        {
            checkpoints[i].RewardValue = (checkpoints[i].AccumulatedDistance / TrackLength) - checkpoints[i-1].AccumulatedReward;
            checkpoints[i].AccumulatedReward = checkpoints[i - 1].AccumulatedReward + checkpoints[i].RewardValue;
        }

    }

    // Calculates the completion percentage of given car with given completed last checkpoint.
    // This method will update the given checkpoint index accordingly to the current position.
    private float GetCompletePerc(AICarController car, ref uint curCheckpointIndex)
    {
        //All checkpoints are captured
        if (curCheckpointIndex >= checkpoints.Length)
        {
            Debug.Log("IWIN");
            return 1;
        }

        //Calculate distance to next checkpoint
        float checkPointDistance = Vector3.Distance(car.transform.position, checkpoints[curCheckpointIndex].transform.position);
        
        //Check if checkpoint can be captured
        if (!checkpoints[curCheckpointIndex].gameObject.activeSelf)
        {
            Debug.Log("Captured checkpoint: " + checkpoints[curCheckpointIndex].gameObject.name);
            curCheckpointIndex++;
            car.CheckpointCaptured(); //Inform car that it captured a checkpoint
            return GetCompletePerc(car, ref curCheckpointIndex); //Recursively check next checkpoint
        }
        else
        {
            //Return accumulated reward of last checkpoint + reward of distance to next checkpoint
            float extra = 0f;
            if (curCheckpointIndex + 1 < checkpoints.Length) extra = checkpoints[curCheckpointIndex + 1].GetRewardValue(checkPointDistance);
            return checkpoints[curCheckpointIndex].AccumulatedReward + extra;
        }
    }

}
