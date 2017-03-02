using UnityEngine;
using UnityEngine.UI;

public class ZonePanel : MonoBehaviour {
    public Toggle alertToggle;

    private SyncedExclusionZone ez;

    private void Awake()
    {
        if (alertToggle == null) Debug.LogError("alert Toggle is not defined");
    }

    public void OnAlertToggleChangeHandler(bool val)
    {
        if(ez != null ) ez.alert = val;
    }

    private void OnEnable()
    {
        ez = AssetPanel.main.selectedAsset.GetComponent<SyncedExclusionZone>();
        if(ez != null)
        {
            if (alertToggle != null)
            {
                alertToggle.isOn = ez.alert;
            }
            else
            {
                Debug.LogError("Missing alertToggleValue");
            }
        }
        
    }
}
