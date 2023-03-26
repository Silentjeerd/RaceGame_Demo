using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownChange : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdownOptions; //dropdownbox object.
    public RawImage image; //image object underneath the dropdownbox.

    void Awake()
    {
        //Add listener for when the value of the Dropdown changes, to take action
        dropdownOptions.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdownOptions);
        });
    }

    /// <summary>
    /// The function that gets called when a new option is selected in the dropdownmenu.
    /// This would change the image beneath the dropdown and adjust the gameSettings.
    /// this is a placeholder for the time being since i didn't expand the cars/tracks with additional options.
    /// </summary>
    /// <param name="change"></param> The selected option within the dropdownbox.
    void DropdownValueChanged(TMPro.TMP_Dropdown change)
    {
        string m_Message = change.options[change.value].text;
        image.texture = Resources.Load<Texture>("Sprites/" + m_Message);
        switch (change.name)
        {
            case "DropdownCar":
                StartUI.Instance.gameSettings.selectedCar = m_Message;
                break;
            case "DropdownTrack":
                StartUI.Instance.gameSettings.selectedTrack = m_Message;
                break;
            default:
                break;
        }
    }
}
