using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class ExclusionZoneManager : MonoBehaviour
    {
    public List<ExclusionZone> ExclusionZones = new List<ExclusionZone>();
    public Material DangerMaterial;
    public Material WarningMaterial;

    IEnumerable<GameObject> ListOfActors = new List<GameObject>();

    private bool exclusionZonesVisible = true;

    public void ToggleZones()
    {
        //TurnZonesOn/OFF
    }
        // Use this for initialization
        void Start()
        {
        if (WorkerManager.main == null) Debug.LogError("missing Worker Manager");
        }

        // Update is called once per frame
        void Update()
        {
            ListOfActors = WorkerManager.main.listOfWorkers;
        }
    }

