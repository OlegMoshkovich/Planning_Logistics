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
    public GameObject hazardPanel;

    private SyncedHazard sh;
	void Start () {
        sh = this.gameObject.GetComponent<SyncedHazard>();
	}


    public void updateFieldsFromSynchedHazard()
    {
        title.text = sh.name;
        description.text = sh.sh_description;
        typeDropdown.value = (int)sh.sh_type;
        timeCreatedText.text = "Created on " + sh.sa_timeCreated;
        timeChangedText.text = "Last changed on " + sh.sa_timeCreated;
        changedByText.text = "Last Changed by " + sh.sa_changedBy;
        status.value = (int)sh.sh_status;
    }

    public void updateSynchedHazardFromFields()
    {
        sh.name = title.text;
        sh.sh_description = description.text;
        sh.sh_type = (HazardType) typeDropdown.value;
        sh.sa_timeCreated = timeCreatedText.text;
        sh.sa_timeCreated = timeChangedText.text;
        sh.sa_changedBy = changedByText.text;
        sh.sh_status = (Status)status.value;
    }
}
