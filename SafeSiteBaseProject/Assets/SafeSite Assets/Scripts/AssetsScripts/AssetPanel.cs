using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class AssetPanel : MonoBehaviour
{
    public static AssetPanel main;
    public Text nameText;
    //SyncedAssetPanel
    public Dropdown movementTypeDropdown;

    public GameObject selectedAsset;
    //Telemetry Panels
    public GameObject telemetryPanel;
    public GameObject addTelemetryPanel;
    

    //RTLS Panels
    public GameObject rtlsPanel;
    public GameObject addRTLSPanel;

    private void Awake()
    {
        main = this;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //If new asset Selected, refresh panel and assign new selected object
                if (selectedAsset != hit.transform.gameObject)
                {
                    gameObject.active = false;
                    gameObject.active = true;
                    selectedAsset = hit.transform.gameObject;
                }
                
                //Activate Panel
                this.GetComponentInParent<Canvas>().enabled = true;
                
                ////////////////////////////////////
                /// Update Properties of Panel/////
                /// ////////////////////////////////

                //Update Data
                nameText.text = selectedAsset.name;

                //Check if Synced Asset
                if (selectedAsset.GetComponent<SyncedAsset>() != null)
                {
                    movementTypeDropdown.gameObject.active = true;
                    movementTypeDropdown.value = (int)selectedAsset.GetComponent<SyncedAsset>().movement;
                    movementTypeDropdown.GetComponent<ChangeMovement>().target = selectedAsset;
                }
                else
                {
                    movementTypeDropdown.gameObject.active = false;
                }

                //Check if Asset has Telemetry

                DisplayTelemetry();

                //Check if RTLS
                DisplayRTLS();
            }
            //No asset selected
            else
            {
                this.GetComponentInParent<Canvas>().enabled = false;
                addTelemetryPanel.active = false;
                addRTLSPanel.active = false;
            }
        }
        
    }

    private void DisplayRTLS()
    {
        if (selectedAsset.GetComponent<RTLSMovement>() != null)
        {
            if (selectedAsset.GetComponent<RTLSMovement>().frequency != "")
            {
                addRTLSPanel.active = false;
                rtlsPanel.active = true;
                rtlsPanel.GetComponent<RTLSPanel>().rtls = selectedAsset.GetComponent<RTLSMovement>();
            }
            else
            {
                rtlsPanel.active = false;
                addRTLSPanel.active = true;

            }
        }
        else
        {
            rtlsPanel.active = false;
            addRTLSPanel.active = true;
        }

    }

    private void DisplayTelemetry()
    {
        if (selectedAsset.GetComponent<HCSTelemetry>() != null)
        {
            if (selectedAsset.GetComponent<HCSTelemetry>().macAddress != "")
            {
                addTelemetryPanel.active = false;
                telemetryPanel.active = true;
                telemetryPanel.GetComponent<TelemetryPanel>().hcsTelemetry = selectedAsset.GetComponent<HCSTelemetry>();
            }
            else
            {
                telemetryPanel.active = false;
                addTelemetryPanel.active = true;

            }
        }
        else
        {
            telemetryPanel.active = false;
            addTelemetryPanel.active = true;
        }
    }
}
