/// Author: Samuel Arzt
/// Date: March 2017

#region Includes
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

/// <summary>
/// Singleton class managing the overall simulation.
/// </summary>
public class GameStateManager : MonoBehaviour
{

    // The name of the track to be loaded
    [SerializeField]
    public string TrackName;

    /// <summary>
    /// The UIController object.
    /// </summary>
    public UIController UIController
    {
        get;
        set;
    }

    public static GameStateManager Instance
    {
        get;
        private set;
    }

    private AICarController prevBest, prevSecondBest;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameStateManagers in the Scene.");
            return;
        }
        Instance = this;

        //Load gui scene
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);

        //Load track
        SceneManager.LoadScene(TrackName, LoadSceneMode.Additive);
    }

    void Start ()
    {
        EvolutionManager.Instance.StartEvolution();
	}

}
