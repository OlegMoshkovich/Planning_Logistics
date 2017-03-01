using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AssetPanel : MonoBehaviour
{
    public static AssetPanel main;
    public Text nameText;
    //SyncedAssetPanel
    public GameObject movementButtons;

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

    private Canvas canvas;

    private void Awake()
    {
        main = this;
        //Check all elements are definded
        if (nameText == null || movementButtons == null || telemetryPanel == null ||
            addTelemetryPanel == null || rtlsPanel == null || addRTLSPanel == null ||
            zonesPanel == null) Debug.LogError("Missing references in Asset Panel");

        canvas = GetComponentInParent<Canvas>();

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
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
            selectedAsset = hit.transform.gameObject;
            //Activate Panel
            canvas.enabled = true;

            ////////////////////////////////////
            /// Update Properties of Panel/////
            /// ////////////////////////////////

            //Update Data
            nameText.text = selectedAsset.name;

            //Check if Synced Asset
            SyncedAsset sa = selectedAsset.GetComponent<SyncedAsset>();
            if (sa != null)
            {
                if (!sa.sa_alwaysStatic) movementButtons.gameObject.SetActive(true);

                //Check if RTLS
                DisplayRTLS();
            }
            else
            {
                movementButtons.gameObject.SetActive(false);
                addRTLSPanel.SetActive(false);
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
           canvas.enabled = false;
            addTelemetryPanel.SetActive(false);
        }
    }

    private void DisplayRTLS()
    {
        var rtlsMovement = selectedAsset.GetComponent<RTLSMovement>();
        if (rtlsMovement != null)
        {
            if (rtlsMovement.frequency != "")
            {
                addRTLSPanel.SetActive(false);
                rtlsPanel.SetActive(true);
                rtlsPanel.GetComponent<RTLSPanel>().rtls = rtlsMovement;
            }
            else
            {
                rtlsPanel.SetActive(false);
                addRTLSPanel.SetActive(true);
            }
        }
        else
        {
            rtlsPanel.SetActive(false);
            addRTLSPanel.SetActive(true);
        }
    }

    private void DisplayTelemetry()
    {
        var hcsTelemetry = selectedAsset.GetComponent<HCSTelemetry>();
        if (hcsTelemetry != null)
        {
            if (hcsTelemetry.macAddress != "")
            {
                addTelemetryPanel.SetActive(false);
                telemetryPanel.SetActive(true);
                telemetryPanel.GetComponent<TelemetryPanel>().hcsTelemetry = hcsTelemetry;
            }
            else
            {
                telemetryPanel.SetActive(false);
                addTelemetryPanel.SetActive(true);

            }
        }
        else
        {
            telemetryPanel.SetActive(false);
            addTelemetryPanel.SetActive(true);
        }
    }

    private void DisplayZoneInfo()
    {
        if(selectedAsset.GetComponent<SyncedExclusionZone>() != null)
        {
            zonesPanel.SetActive(true);
        }
        else
        {
            zonesPanel.SetActive(false);
        }
    }
    //Function that sizes panel depending on content
    public void updatePanelsLayout()
    {
        float sumOfHeights = 20; //Buffer
        RectTransform[] rectTransforms = gameObject.transform.GetComponentsInChildren<RectTransform>();
        foreach(RectTransform rectT in rectTransforms)
        {
            if (rectT.gameObject.activeSelf && rectT.gameObject.transform.parent == gameObject.transform && (rectT!= this.gameObject.GetComponent<RectTransform>()))
            {
                rectT.anchoredPosition = new Vector2(rectT.anchoredPosition.x, -sumOfHeights);
                sumOfHeights += rectT.sizeDelta.y;
                sumOfHeights += 20; // Buffer between UI elements           
            }  
        }   
        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.gameObject.GetComponent<RectTransform>().sizeDelta.x, sumOfHeights);
    }
}
