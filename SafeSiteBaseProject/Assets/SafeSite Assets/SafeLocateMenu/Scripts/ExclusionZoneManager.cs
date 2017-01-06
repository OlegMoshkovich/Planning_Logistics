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
    void Start()
    {
        exclusionZones = new GameObject();
        exclusionZones.name = "Exclusion Zones";
        exclusionZones.AddComponent<Folder>();
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
    public void OnExclusionZoneEnter_Handler(Collision collision)
    {
        Debug.Log(collision.collider.name + " + GameObject: " + collision.gameObject.name);
        collision.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = exclusionZoneDangerMaterial;
        //Activate worker telemetry alarm
        if (collision.gameObject.GetComponent<WorkerTelemetry>() !=null) {
            mqttManager.main.ActivateAlarm(collision.gameObject.GetComponent<WorkerTelemetry>().macAddress);
        }
        else
        {
            Debug.Log("Worker doesn't have WorkerTelemetry class Assigned");
        }
    }
    public void OnExclusionZoneStay_Handler(Collision collision)
    {
        if (collision.gameObject.GetComponent<WorkerTelemetry>() != null)
        {
            mqttManager.main.ActivateAlarm(collision.gameObject.GetComponent<WorkerTelemetry>().macAddress);
        }
    }
    public void OnExclusionZoneExit_Handler(Collision collision)
    {
        Debug.Log("ExclusionExit");
        if (collision.gameObject.GetComponent<WorkerTelemetry>() != null)
        {
            mqttManager.main.DeactivateAlarm(collision.gameObject.GetComponent<WorkerTelemetry>().macAddress);
        }
    }
    public void OnWorkerHitExclusionZone ( Collider collider)
    {
        Debug.Log(collider.name);
    }
}
