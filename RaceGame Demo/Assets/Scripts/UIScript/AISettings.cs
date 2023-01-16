using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AISettings : MonoBehaviour
{
    [SerializeField] TMP_InputField agentCount;
    [SerializeField] TMP_InputField generations;
    [SerializeField] Toggle saveGhosts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        if (agentCount.text == "") agentCount.text = "0";
        if (generations.text == "") generations.text = "0";
        StartUI.Instance.gameSettings.playerInput = false;
        StartUI.Instance.gameSettings.agentCount = int.Parse(agentCount.text);
        StartUI.Instance.gameSettings.generations = int.Parse(generations.text);
        StartUI.Instance.gameSettings.saveGhost = saveGhosts.isOn;
        StartUI.Instance.gameSettings.NNTraining = true;
        StartUI.Instance.gameSettings.sensorCount = 13;
    }
}
