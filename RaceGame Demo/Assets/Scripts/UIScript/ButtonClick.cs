using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonClick : MonoBehaviour
{
	public Button button;
	public GameObject Menu;

	private bool playerInput;

	void Start()
	{
		//Button btn = button.GetComponent<Button>();
		button.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
        switch (button.name)
        {
			case "changeMenu":
				StartUI.Instance.ChangeMenu(Menu);
				break;
			
			case "StartAI":
				AISettings();
				SceneManager.LoadScene("GameScene");
				break;
			
			case "StartRace":
				RaceSettings();
				SceneManager.LoadScene("GameScene");
				break;

			case "ExitGame":
				Application.Quit();
				break;

			default:
				break;
        }
	}

	void AISettings()
    {
		string agents = Menu.transform.Find("Agents/agentsInput").GetComponent<TMP_InputField>().text;
		if (agents == "") agents = "0"; //throw error
		StartUI.Instance.gameSettings.agentCount = int.Parse(agents);
		string generations = Menu.transform.Find("Generations/generationsInput").GetComponent<TMP_InputField>().text;
		if (generations== "") generations = "0";//throw error
		StartUI.Instance.gameSettings.generations = int.Parse(generations);

		StartUI.Instance.gameSettings.saveGhost = Menu.transform.Find("GhostToggle").GetComponent<Toggle>().isOn;

		StartUI.Instance.gameSettings.NNTraining = true;
		StartUI.Instance.gameSettings.sensorCount = 13;
		StartUI.Instance.gameSettings.playerInput = false;
		//Debug.Log(Menu.transform.Find("Agents/agentsInput").GetComponent<TMP_InputField>().text);
	}

	void RaceSettings()
    {

		StartUI.Instance.gameSettings.saveGhost = Menu.transform.Find("GhostToggle").GetComponent<Toggle>().isOn;
		StartUI.Instance.gameSettings.playerInput = true;
	}

}
