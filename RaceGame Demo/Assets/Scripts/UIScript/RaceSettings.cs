using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaceSettings : MonoBehaviour
{

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
        StartUI.Instance.gameSettings.playerInput = true;
        StartUI.Instance.gameSettings.saveGhost = saveGhosts.isOn;
    }
}
