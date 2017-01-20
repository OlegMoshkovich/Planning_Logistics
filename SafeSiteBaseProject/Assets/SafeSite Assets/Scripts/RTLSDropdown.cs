using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RTLSDropdown : MonoBehaviour
{
    private Dropdown dropdown;
    // Use this for initialization
    void OnEnable()
    {
        if (GetComponent<Dropdown>() != null)
        {
            dropdown = GetComponent<Dropdown>();
            dropdown.ClearOptions();
            dropdown.options.Add(new Dropdown.OptionData() { text = "none" });
            if (mqttManager.main.listOfQTrackTags.Count > 0)
            {
                foreach (string tagID in mqttManager.main.listOfQTrackTags.Keys)
                {
                    dropdown.options.Add(new Dropdown.OptionData() { text = tagID });
                }
            }
            dropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogError("HCSTagsDropdown class need to be attached to a Dropdown");
        }
    }
}