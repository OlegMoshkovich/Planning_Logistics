using UnityEngine;
using System.Collections.Generic;

public class ExclusionZoneManager : MonoBehaviour
{
    public Material exclusionZoneDangerMaterial;
    public Material exclusionZoneWarningMaterial;
    public List<GameObject> listOfExclusionZones = new List<GameObject>();
    public GameObject exclusionZones;

    public static ExclusionZoneManager main;
    // Use this for initialization
    void Awake()
    {
        main = this;
    }
    public void LoadExclusionZone(string input)
    {
        /*
        GameObject newGO = new GameObject();
        ExclusionZone ez = newGO.AddComponent<ExclusionZone>();
        JsonUtility.FromJsonOverwrite(input,ez);
        */
    }
    void Start()
    {
        exclusionZones = new GameObject();
        exclusionZones.name = "Exclusion Zones";
        TreeViewManager.main.TreeView.Add(exclusionZones);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool AddExclusionZone(GameObject ez)
    {
        if(ez.GetComponent<ExclusionZone>() == null)
        {
            Debug.LogError(ez.name + " Missing Exclusion zone");
            return false;
        }
        else
        {
            //Add Event Listeners MUST FIX, bypased for now by calling directly the handler
            //ez.GetComponent<ExclusionZone>().OnExclusionZoneEnter += OnExclusionZoneEnter_Handler;
            //ez.GetComponent<ExclusionZone>().OnExclusionZoneExit += OnExclusionZoneExit_Handler;
            //Add to list of exclusion Zones
            listOfExclusionZones.Add(ez);

            return true;
        }
    }
    void CreateNewExclusionZone()
    {
        //ExclusionZoneDrawer newExclusionZoneDrawwer = new ExclusionZoneDrawer();
    }
    public void OnExclusionZoneEnter_Handler(Collider collider)
    {
        Debug.Log(collider.name + " + GameObject: " + collider.gameObject.name);
        collider.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = exclusionZoneDangerMaterial;
        //Activate worker telemetry alarm
        if (collider.gameObject.GetComponent<HCSTelemetry>() !=null) {
            mqttManager.main.ActivateAlarm(collider.gameObject.GetComponent<HCSTelemetry>().macAddress);
        }
        else
        {
            Debug.Log("Worker doesn't have WorkerTelemetry class Assigned");
        }
    }
    public void OnExclusionZoneStay_Handler(Collision collision)
    {
        if (collision.gameObject.GetComponent<HCSTelemetry>() != null)
        {
            mqttManager.main.ActivateAlarm(collision.gameObject.GetComponent<HCSTelemetry>().macAddress);
        }
    }
    public void OnExclusionZoneExit_Handler(Collider collider)
    {
        Debug.Log("ExclusionExit");
        if (collider.gameObject.GetComponent<HCSTelemetry>() != null)
        {
            mqttManager.main.DeactivateAlarm(collider.gameObject.GetComponent<HCSTelemetry>().macAddress);
            collider.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = Resources.Load<Material>("WorkerMaterial");
        }
    }
}
