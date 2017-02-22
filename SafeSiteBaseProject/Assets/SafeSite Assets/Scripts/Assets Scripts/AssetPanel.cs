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

    [HideInInspector]
    public GameObject selectedAsset;
    //Telemetry Panels
    public GameObject telemetryPanel;
    public GameObject addTelemetryPanel;
    

    //RTLS Panels
    public GameObject rtlsPanel;
    public GameObject addRTLSPanel;

    //Zones Panel
    public GameObject zonesPanel;

    private void Awake()
    {
        main = this;
        //Check all elements are definded
        if (nameText == null || movementTypeDropdown == null || telemetryPanel == null ||
            addTelemetryPanel == null || rtlsPanel == null || addRTLSPanel == null ||
            zonesPanel == null) Debug.LogError("Missing references in Asset Panel");

    }
    private void Update()
    {
        //if Mouse or touch is outside Panel Raycast to update panel
            if (Input.touchCount == 1 )
            {
                if(!RectTransformUtility.RectangleContainsScreenPoint(this.gameObject.GetComponent<RectTransform>(), Input.GetTouch(0).position, Camera.main))
                {
                    RaycastAndUpdatePanel();
                }
            }
        else if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastAndUpdatePanel();           
        }
        
    }

    public void RaycastAndUpdatePanel()
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
                //selectedAsset = hit.transform.gameObject;
            }
            selectedAsset = hit.transform.gameObject;
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
                if (!selectedAsset.GetComponent<SyncedAsset>().sa_alwaysStatic) movementTypeDropdown.gameObject.active = true;
                movementTypeDropdown.GetComponent<ChangeMovement>().target = selectedAsset;
                movementTypeDropdown.value = (int)selectedAsset.GetComponent<SyncedAsset>().sa_movement;

                //Check if RTLS
                DisplayRTLS();
            }
            else
            {
                movementTypeDropdown.gameObject.active = false;
                addRTLSPanel.active = false;
            }

            //Check if Asset has Telemetry

            DisplayTelemetry();


            //Check if Zones
            DisplayZoneInfo();

            //Size Panel
            updatePanelsLayout();
        }
        //No asset selected
        else
        {
            this.GetComponentInParent<Canvas>().enabled = false;
            addTelemetryPanel.active = false;
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

    private void DisplayZoneInfo()
    {
        if(selectedAsset.GetComponent<SyncedExclusionZone>() != null)
        {
            zonesPanel.active = true;
        }
        else
        {
            zonesPanel.active = false;
        }
    }
    //Function that sizes panel depending on content
    public void updatePanelsLayout()
    {
        float sumOfHeights = 20; //Buffer
        RectTransform[] rectTransforms = gameObject.transform.GetComponentsInChildren<RectTransform>();
        foreach(RectTransform rectT in rectTransforms)
        {
            if (rectT.gameObject.active && rectT.gameObject.transform.parent == gameObject.transform && (rectT!= this.gameObject.GetComponent<RectTransform>()))
            {
                rectT.anchoredPosition = new Vector2(rectT.anchoredPosition.x, -sumOfHeights);
                sumOfHeights += rectT.sizeDelta.y;
                sumOfHeights += 20; // Buffer between UI elements           
            }  
        }   
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.gameObject.GetComponent<RectTransform>().sizeDelta.x, sumOfHeights);
    }
}
