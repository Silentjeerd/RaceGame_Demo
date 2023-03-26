/// Author: Samuel Arzt
/// Date: March 2017


using UnityEngine;


/// <summary>
/// Class representing a controlling container for a 2D physical simulation
/// of a car with 5 front facing sensors, detecting the distance to obstacles.
/// </summary>
public class AICarController : MonoBehaviour
{
    // Used for unique ID generation
    private static int idGenerator = 0;

    /// <summary>
    /// Returns the next unique id in the sequence.
    /// </summary>
    private static int NextID
    {
        get { return idGenerator++; }
    }

    /// <summary>
    /// Maximum delay in seconds between the collection of two checkpoints until this car dies.
    /// </summary>
    private const float MAX_CHECKPOINT_DELAY = 7;

    /// <summary>
    /// The underlying AI agent of this car.
    /// </summary>
    public Agent Agent
    {
        get;
        set;
    }

    /// <summary>
    /// Current score of the Agent.
    /// </summary>
    public float CurrentCompletionReward
    {
        get { return Agent.Genotype.Evaluation; }
        set { Agent.Genotype.Evaluation = value; }
    }

    /// <summary>
    /// The movement component of this car.
    /// </summary>
    public PlayerCarController Movement
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

    /// <summary>
    /// Instantiation 
    /// </summary>
    void Awake()
    {
        Movement = GetComponent<PlayerCarController>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        sensors = GetComponentsInChildren<Sensor>();
        this.name = "Agent " + NextID;
        this.gameObject.SetActive(false);
        Movement.HitWall += Die;
    }

    /// <summary>
    /// Restarts the car, its Agent and activates the Sensors.
    /// </summary>
    public void Restart()
    {
        this.gameObject.SetActive(true);
        Movement.enabled = true;
        Movement.RestartStreamWriter();
        timeSinceLastCheckpoint = 0;

        foreach (Sensor s in sensors)
            s.Show();

        Agent.Reset();
        this.enabled = true;
    }

    /// <summary>
    /// Updates the timeSinceLastCheckpoint.
    /// Reads the Sensors on the car and gives the values to the NeuralNetwork of the Agent.
    /// Gives the returned inputs to the Movement component (PlayerCarController).
    /// Checks if the Agent has to be terminated.
    /// </summary>
    void FixedUpdate()
    {
        timeSinceLastCheckpoint += Time.deltaTime;
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

    /// <summary>
    /// Stops the Car and terminates the Agent.
    /// </summary>
    private void Die()
    {
        this.gameObject.SetActive(false);
        this.enabled = false;
        Movement.Stop();
        Movement.enabled = false;

        foreach (Sensor s in sensors)
            s.Hide();

        Agent.Kill();
    }

    /// <summary>
    /// Gets called from PlayerCarController upon capturing a checkpoint to reset the timer.
    /// </summary>
    public void CheckpointCaptured()
    {
        timeSinceLastCheckpoint = 0;
    }

}
