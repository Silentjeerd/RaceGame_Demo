using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadReadyAgents : MonoBehaviour
{

    public static LoadReadyAgents Instance
    {
        get;
        private set;
    }

    public int PopulationSize
    {
        get;
        private set;
    }

    private List<Genotype> currentPopulation;
    string filepath = "C:/Users/tjeer/Desktop/Minor Unity Project/RaceGameDemo/RaceGame Demo/Evaluation1/";//1.txt";
    private uint[] FNNTopology;
    private List<Agent> agents = new List<Agent>();

    public void startEvo()
    {
        FNNTopology = new uint[] { GameManager.Instance.settings.sensorCount, 8 };

        PopulationSize = 2;
        currentPopulation = new List<Genotype>(PopulationSize);
        //Initialise empty population
        //Genotype genes = new Genotype();
        //genes.LoadFromFile(filepath);
        for (int i = 0; i < PopulationSize; i++)
            //currentPopulation.Add(new Genotype(new float[genotypeParamCount]));
            currentPopulation.Add(Genotype.LoadFromFile(filepath + PopulationSize + ".txt"));

        foreach (Genotype genotype in currentPopulation)
            agents.Add(new Agent(genotype, MathHelper.SoftSignFunction, FNNTopology));

        TrackManager.Instance.SetCarAmount(agents.Count);
        IEnumerator<AICarController> carsEnum = TrackManager.Instance.GetCarEnumerator();

        for (int i = 0; i < agents.Count; i++)
        {

            if (!carsEnum.MoveNext())
            {
                Debug.LogError("Cars enum ended before agents.");
                break;
            }
            carsEnum.Current.Agent = agents[i];
            //AgentsAliveCount++;
            // agents[i].AgentDied += OnAgentDied;
        }
        //append the savefilpath
        //GameManager.Instance.changeSaveFileFolder("Generation" + GenerationCount + "/");
        //Debug.Log("Starting Evaluation of generation : " + GenerationCount + " with " + AgentsAliveCount + " agents");
        TrackManager.Instance.Restart();

    }
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
