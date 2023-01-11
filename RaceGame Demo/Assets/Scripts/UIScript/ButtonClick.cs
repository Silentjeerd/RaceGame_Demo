using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
	public Button button;
	public GameObject Menu;

	void Start()
	{
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		if(button.name == "Start")
        {
			SceneManager.LoadScene("RaceScene");
        }
		else if(button.name == "StartTraining"){
			SceneManager.LoadScene("NNScene");
        }else
        {
			StartUI.Instance.ChangeMenu(Menu);
		}

	}
}
