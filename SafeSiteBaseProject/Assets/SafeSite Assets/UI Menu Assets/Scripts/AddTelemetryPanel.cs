using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddTelemetryPanel : MonoBehaviour {
    public Button addTelemetryButton;
    public Text tagIDText;
    public Dropdown hcsTagDropdown;
    public GameObject TelemetryPanel;
	// Use this for initialization
	void Start () {
        hcsTagDropdown.onValueChanged.AddListener(delegate {
            myDropdownValueChangedHandler(hcsTagDropdown);
        });
     
    }
    private void OnDestroy()
    {
        hcsTagDropdown.onValueChanged.RemoveAllListeners();
    }

    private void OnEnable()
    {
        addTelemetryButton.gameObject.active = true;
        tagIDText.gameObject.active = false;
        hcsTagDropdown.gameObject.active = false;
    }

    private void myDropdownValueChangedHandler(Dropdown target)
    {
        if(target.value != 0)
        {
            if (AssetPanel.main.selectedAsset.GetComponent<HCSTelemetry>() == null) { AssetPanel.main.selectedAsset.AddComponent<HCSTelemetry>(); }
            HCSTelemetry tag = AssetPanel.main.selectedAsset.GetComponent<HCSTelemetry>();
            tag.macAddress = target.options[target.value].text;

            this.gameObject.active = false;
            TelemetryPanel.active = true;
        }
        
    }
    private void OnDisable()
    {
        addTelemetryButton.gameObject.active = true;
        tagIDText.gameObject.active = false;
        hcsTagDropdown.gameObject.active = false;
    }


}
