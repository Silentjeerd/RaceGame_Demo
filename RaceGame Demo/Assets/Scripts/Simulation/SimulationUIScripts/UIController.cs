/// Author: Samuel Arzt
/// Date: March 2017

#region Includes
using UnityEngine;
using UnityEngine.UI;
#endregion

/// <summary>
/// Class for controlling the overall GUI.
/// </summary>
public class UIController : MonoBehaviour
{

    /// <summary>
    /// The parent canvas of all UI elements.
    /// </summary>
    public Canvas Canvas
    {
        get;
        private set;
    }

    private UISimulationController simulationUI;
    private UIStartMenuController startMenuUI;

    void Awake()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.UIController = this;

        Canvas = GetComponent<Canvas>();
        simulationUI = GetComponentInChildren<UISimulationController>(true);
        startMenuUI = GetComponentInChildren<UIStartMenuController>(true);

        simulationUI.Show();
    }

    /// <summary>
    /// Sets the CarController from which to get the data from to be displayed.
    /// </summary>
    /// <param name="target">The CarController to display the data of.</param>
    public void SetDisplayTarget(AICarController target)
    {
        simulationUI.Target = target;
    }

}
