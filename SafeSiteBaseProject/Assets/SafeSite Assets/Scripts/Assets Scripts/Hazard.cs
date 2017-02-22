using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(SyncedHazard))]
public class Hazard : MonoBehaviour {
    public InputField title;
    public Dropdown typeDropdown;
    public Text timeCreatedText;
    public Text timeChangedText;
    public Text changedByText;
    public InputField description;
    public Dropdown status;

    private SyncedHazard sh;

    void Start () {
        sh = this.gameObject.GetComponent<SyncedHazard>();
	}


    public void updateFieldsFromSynchedHazard()
    {
        if (sh != null)
        {
            if (title != null) title.text = sh.name;
            if (description != null) description.text = sh.sh_description;
            if (typeDropdown != null) typeDropdown.value = (int)sh.sh_type;
            if (timeCreatedText != null) timeCreatedText.text = "Created on " + sh.sa_timeCreated;
            if (timeChangedText != null) timeChangedText.text = "Last changed on " + sh.sa_timeCreated;
            if (changedByText != null) changedByText.text = "Last Changed by " + sh.sa_changedBy;
            if (status != null) status.value = (int)sh.sh_status;
        }
        else Debug.Log("Hazard is missing SynchedHazard Component");     
    }

    public void updateSynchedHazardFromFields()
    {
        if (sh != null)
        {
            if (title != null) sh.name = title.text;
            if (description != null) sh.sh_description = description.text;
            if (typeDropdown != null)sh.sh_type = (HazardType)typeDropdown.value;
            if (timeCreatedText != null) sh.sa_timeCreated = timeCreatedText.text;
            if (timeChangedText != null) sh.sa_timeChanged = timeChangedText.text;
            if (changedByText != null) sh.sa_changedBy = changedByText.text;
            if (status != null) sh.sh_status = (Status)status.value;
        }
        else Debug.Log("Hazard is missing Synched Hazard");
    }
}
