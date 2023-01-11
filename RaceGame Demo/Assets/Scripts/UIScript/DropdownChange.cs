using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownChange : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdownoptions;
    public RawImage image;
    //public GameObject dropdownObj;
    //Dropdown m_Dropdown;
    Text m_Text;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Dropdown GameObject
        //m_Dropdown = dropdownObj.GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        dropdownoptions.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdownoptions);
        });

        //Initialise the Text to say the first value of the Dropdown
        //m_Text.text = "First Value : " + dropdownoptions.value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DropdownValueChanged(TMPro.TMP_Dropdown change)
    {
        string m_Message = change.options[change.value].text;
        image.texture = Resources.Load<Texture>("Sprites/" + m_Message);
        if (change.name == "DropdownCar") StartUI.Instance.SetCarChoice(m_Message); else StartUI.Instance.SetTrackChoice(m_Message);
    }

}
