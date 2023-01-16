/// Author: Samuel Arzt
/// Date: March 2017

#region Includes
using UnityEngine;
#endregion

/// <summary>
/// Class representing a controlling container for a 2D physical simulation
/// of a car with 5 front facing sensors, detecting the distance to obstacles.
/// </summary>
public class CarController : MonoBehaviour
{
    #region Members
    #region IDGenerator
    // Used for unique ID generation
    private static int idGenerator = 0;
    /// <summary>
    /// Returns the next unique id in the sequence.
    /// </summary>
    private static int NextID
    {
        get { return idGenerator++; }
    }
    #endregion

    // Maximum delay in seconds between the collection of two checkpoints until this car dies.
    private const float MAX_CHECKPOINT_DELAY = 7;

    /// <summary>
    /// The underlying AI agent of this car.
    /// </summary>
    public Agent Agent
    {
        get;
        set;
    }

    public float CurrentCompletionReward
    {
        get { return Agent.Genotype.Evaluation; }
        set { Agent.Genotype.Evaluation = value; }
    }

    /// <summary>
    /// The movement component of this car.
    /// </summary>
    public MyCarMovement Movement
    {
        get;
        private set;
    }

    /// <summary>
    /// The current inputs for controlling the CarMovement component.
    /// </summary>
    public float[] CurrentControlInputs
    {
        get { return Movement.CurrentInputs; }
    }

    /// <summary>
    /// The cached SpriteRenderer of this car.
    /// </summary>
    public SpriteRenderer SpriteRenderer
    {
        get;
        private set;
    }

    private Sensor[] sensors;
    private float timeSinceLastCheckpoint;
    #endregion

    #region Constructors
    void Awake()
    {
        //Cache components
        Movement = GetComponent<MyCarMovement>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        sensors = GetComponentsInChildren<Sensor>();
        this.name = "Agent " + NextID;
        this.gameObject.SetActive(false);
    }

    void Start()
    {
       Movement.HitWall += Die;  
    }
    #endregion

    #region Methods
    /// <summary>
    /// Restarts this car, making it movable again.
    /// </summary>
    public void Restart()
    {
        //Debug.Log("restarting car: " + this.name);
        this.gameObject.SetActive(true);
        Movement.enabled = true;
        Movement.Restart();
        timeSinceLastCheckpoint = 0;

        foreach (Sensor s in sensors)
            s.Show();

        Agent.Reset();
        this.enabled = true;
    }

    // Unity method for normal update
    void Update()
    {
        timeSinceLastCheckpoint += Time.deltaTime;
    }

    // Unity method for physics update
    void FixedUpdate()
    {
        //Get readings from sensors
        double[] sensorOutput = new double[sensors.Length];
        for (int i = 0; i < sensors.Length; i++)
        {
            sensorOutput[i] = sensors[i].Output;
        }

        double[] controlInputs = Agent.FNN.ProcessInputs(sensorOutput);
        float[] newInputs = new float[2];
        newInputs[0] = (float)controlInputs[0];
        newInputs[1] = (float)controlInputs[1];

        Movement.SetInputs(newInputs);
        if (timeSinceLastCheckpoint > MAX_CHECKPOINT_DELAY || Agent.Genotype.Evaluation == 1) Die();

    }

    // Makes this car die (making it unmovable and stops the Agent from calculating the controls for the car).
    private void Die()
    {
        Debug.Log("Agent has died, score was :" + this.CurrentCompletionReward);
        this.gameObject.SetActive(false);
        this.enabled = false;
        Movement.Stop();
        Movement.enabled = false;

        foreach (Sensor s in sensors)
            s.Hide();

        Agent.Kill();
    }

    public void CheckpointCaptured()
    {
        timeSinceLastCheckpoint = 0;
    }
    #endregion


}
