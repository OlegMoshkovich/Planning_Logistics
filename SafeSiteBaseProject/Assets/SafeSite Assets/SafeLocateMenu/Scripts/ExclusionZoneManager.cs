using UnityEngine;
using System.Collections.Generic;

public class ExclusionZoneManager : MonoBehaviour
{
    public Material exclusionZoneDangerMaterial;
    public Material exclusionZoneWarningMaterial;
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

    public bool AddExclusionZone(GameObject ez)
    {
        if(ez.GetComponent<SyncedExclusionZone>() == null)
        {
            Debug.LogError(ez.name + " Missing Exclusion zone");
            return false;
        }
        else
        {
            return true;
        }
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
