using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GhostSelection : MonoBehaviour
{
    [SerializeField] private Transform ContentContainer; //The container that holds the list of items.
    [SerializeField] private GameObject ItemPrefab;  //A prefab of a list item, set in the unity editor.

    /// <summary>
    /// calls the fillList function when the menu gets created.
    /// </summary>
    void Awake()
    {
        fillListFromFolder("Assets/Resources/TxtGhosts/" + StartUI.Instance.gameSettings.selectedTrack + "/");
    }

    /// <summary>
    /// Fills the list container with all available ghostfile names in the given folder.
    /// Adds a toggle to each item so it can be selected.
    /// </summary>
    /// <param name="folderPath"></param> Folderpath of the ghostfiles for the currently selected track.
    void fillListFromFolder(string folderPath)
    {
        string[] dirs = Directory.GetFiles(folderPath, "*.txt");

        foreach (var file in dirs)
        {
            string ghostName = file.Replace(folderPath, "");
            ghostName = ghostName.Replace(".txt", "");

            GameObject listViewItem = (GameObject)Instantiate(ItemPrefab);
            Text listViewItemText = listViewItem.GetComponentInChildren<Text>();
            listViewItemText.text = ghostName;

            listViewItem.transform.SetParent(ContentContainer);
            listViewItem.transform.localScale = Vector2.one;

            Toggle t = listViewItem.GetComponentInChildren<Toggle>();
            t.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(t, listViewItemText);
            });
        }
    }

    /// <summary>
    /// This function get called when a toggle field is clicked.
    /// Changes the text color of the item and adds/removes it from the list that has to be loaded for the race.
    /// </summary>
    /// <param name="toggle"></param> The toggle, used to see if its ticked or unticked.
    /// <param name="ghostName"></param> Textfield which contains the name of the txtfile.
    void ToggleValueChanged(Toggle toggle, Text ghostName)
    {
        if (toggle.isOn)
        {
            ghostName.color = Color.green;
            StartUI.Instance.gameSettings.ghostFiles.Add(ghostName.text);
        }
        else
        {
            ghostName.color = Color.black;
            StartUI.Instance.gameSettings.ghostFiles.Remove(ghostName.text);
        }
    }
}
