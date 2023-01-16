using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GhostPicker : MonoBehaviour
{
    [SerializeField] private Transform ContentContainer;
    [SerializeField] private GameObject ItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        string folderPath = "Assets/Resources/TxtGhosts/" + StartUI.Instance.gameSettings.selectedTrack + "/";
        string[] dirs = Directory.GetFiles(folderPath, "*.txt");
        foreach(var file in dirs)
        {
            string itemName = file.Replace(folderPath, "");
            itemName = itemName.Replace(".txt", "");
            GameObject item = (GameObject)Instantiate(ItemPrefab);
            Text text = item.GetComponentInChildren<Text>();
            text.text = itemName;
            item.transform.SetParent(ContentContainer);
            item.transform.localScale = Vector2.one;
            Toggle t = item.GetComponentInChildren<Toggle>();
            t.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(t,text);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleValueChanged(Toggle toggle,Text text)
    {
        if (toggle.isOn)
        {
            text.color = Color.green;
            StartUI.Instance.gameSettings.ghostFiles.Add(text.text);
        }
        else 
        { 
            text.color = Color.black;
            StartUI.Instance.gameSettings.ghostFiles.Remove(text.text);
        }
    }
}
