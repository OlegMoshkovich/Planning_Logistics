using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddRTLSPanel : MonoBehaviour
{
    public Button addRTLSButton;
    public Text tagIDText;
    public Dropdown RTLSDropdown;
    public GameObject RTLSPanel;
    // Use this for initialization
    void Start()
    {
        RTLSDropdown.onValueChanged.AddListener(delegate {
            myDropdownValueChangedHandler(RTLSDropdown);
        });

    }
    private void OnDestroy()
    {
        RTLSDropdown.onValueChanged.RemoveAllListeners();
    }

    private void OnEnable()
    {
        addRTLSButton.gameObject.active = true;
        tagIDText.gameObject.active = false;
        RTLSDropdown.gameObject.active = false;
    }

    private void myDropdownValueChangedHandler(Dropdown target)
    {
        if (target.value != 0)
        {
            if (AssetPanel.main.selectedAsset.GetComponent<RTLSMovement>() == null) { AssetPanel.main.selectedAsset.AddComponent<RTLSMovement>(); }
            RTLSMovement rtls = AssetPanel.main.selectedAsset.GetComponent<RTLSMovement>();
            rtls.frequency = target.options[target.value].text;

            this.gameObject.active = false;
            RTLSPanel.active = true;
        }

    }
    private void OnDisable()
    {
        addRTLSButton.gameObject.active = true;
        tagIDText.gameObject.active = false;
        RTLSDropdown.gameObject.active = false;
    }

}
