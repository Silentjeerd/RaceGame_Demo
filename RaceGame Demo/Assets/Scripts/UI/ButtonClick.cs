using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonClick : MonoBehaviour
{
	public Button button; //The button this script is attached to, set in the unity editor.
	public GameObject Menu; //The menu the button is located in, set in the unity editor.

	/// <summary>
    /// Adds the TaskOnClick to the button upon creation.
    /// </summary>
	void Start()
	{
		button.onClick.AddListener(TaskOnClick);
	}

	/// <summary>
    /// OnClick function of the button.
    /// Switch case based on the buttons name.
    /// </summary>
	void TaskOnClick()
	{
        switch (button.name)
        {
			case "nextMenu":
				Command pushCommand = new MenuSwapCommand(this.transform.parent.gameObject, Menu);
				StartUI.Instance.commandStack.Push(pushCommand);
				pushCommand.Execute();
				break;

			case "previousMenu":
				Command popCommand = (Command)StartUI.Instance.commandStack.Pop();
				popCommand.UnExecute();
				break;

			case "StartAI":
				AISettings();
				break;
			
			case "StartRace":
				RaceSettings();
				break;

			case "ExitGame":
				Application.Quit();
				break;

			default:
				break;
        }
	}

	/// <summary>
    /// Sets all the settings required to start the training of the AI.
    /// Sets base values if they are left empty.
    /// </summary>
	void AISettings()
    {
		string agents = Menu.transform.Find("Agents/agentsInput").GetComponent<TMP_InputField>().text;
		if (agents == "") agents = "10";
		StartUI.Instance.gameSettings.agentCount = int.Parse(agents);

		string generations = Menu.transform.Find("Generations/generationsInput").GetComponent<TMP_InputField>().text;
		if (generations == "") generations = "20";
		StartUI.Instance.gameSettings.generations = int.Parse(generations);

		StartUI.Instance.gameSettings.NNTraining = true;
		StartUI.Instance.gameSettings.sensorCount = 13;
		StartUI.Instance.gameSettings.playerInput = false;
		SaveAndLoad();
	}

	/// <summary>
    /// Sets the settings required to race as a player.
    /// </summary>
	void RaceSettings()
    {
		StartUI.Instance.gameSettings.NNTraining = false;
		StartUI.Instance.gameSettings.playerInput = true;
		SaveAndLoad();
	}

	/// <summary>
    /// Saves the settings and loads the gamescene.
    /// </summary>
	void SaveAndLoad()
    {
		StartUI.Instance.Save();
		SceneManager.LoadScene("GameScene");
	}

}
