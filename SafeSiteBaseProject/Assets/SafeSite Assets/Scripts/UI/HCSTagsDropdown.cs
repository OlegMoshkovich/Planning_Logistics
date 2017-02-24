using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HCSTagsDropdown : MonoBehaviour {
    private Dropdown dropdown;
    // Use this for initialization
    protected void Awake()
    {
        dropdown = GetComponent<Dropdown>();
        if (dropdown == null) Debug.LogError("Dropdown field must be iniated");
    }

    void OnEnable () {
        if (dropdown != null)
        {
            dropdown.ClearOptions();
            dropdown.options.Add(new Dropdown.OptionData() { text = "none" });
            if (mqttManager.main.listOfHCSTags.Count > 0)
            {
                foreach (string tagID in mqttManager.main.listOfHCSTags.Keys)
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