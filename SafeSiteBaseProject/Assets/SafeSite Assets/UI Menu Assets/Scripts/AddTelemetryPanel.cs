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
        if (addTelemetryButton != null && tagIDText != null && hcsTagDropdown != null)
        {
            addTelemetryButton.gameObject.SetActive(true);
            tagIDText.gameObject.SetActive(false);
            hcsTagDropdown.gameObject.SetActive(false);
        }
        else Debug.Log("Missing fields in Add Telemetry Panel");
    }

    private void myDropdownValueChangedHandler(Dropdown target)
    {
        if(target.value != 0)
        {
            if (AssetPanel.main.selectedAsset.GetComponent<HCSTelemetry>() == null) { AssetPanel.main.selectedAsset.AddComponent<HCSTelemetry>(); }
            HCSTelemetry tag = AssetPanel.main.selectedAsset.GetComponent<HCSTelemetry>();
            tag.macAddress = target.options[target.value].text;

            this.gameObject.SetActive(false);
            TelemetryPanel.SetActive(true);
        }
        
    }
    private void OnDisable()
    {
        addTelemetryButton.gameObject.SetActive( true);
        tagIDText.gameObject.SetActive(false);
        hcsTagDropdown.gameObject.SetActive(false);
    }


}
