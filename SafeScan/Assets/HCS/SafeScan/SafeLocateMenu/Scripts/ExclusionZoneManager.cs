using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class ExclusionZoneManager : MonoBehaviour
    {
        public Material exclusionZoneDangerMaterial;
        public Material exclusionZoneWarningMaterial;
        public List<ExclusionZone> listOfExclusionZones= new List<ExclusionZone>();
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
        void CreateNewExclusionZone()
        {
            //ExclusionZoneDrawer newExclusionZoneDrawwer = new ExclusionZoneDrawer();
        }
    }
