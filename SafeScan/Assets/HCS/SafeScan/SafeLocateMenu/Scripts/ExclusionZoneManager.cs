using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class ExclusionZoneManager : MonoBehaviour
    {
        public Material exclusionZoneMaterial;
        public List<ExclusionZone> listOfExclusionZones= new List<ExclusionZone>();

        public static ExclusionZoneManager main = new ExclusionZoneManager();
        // Use this for initialization
        void Start()
        {
            main = this;
        }

        // Update is called once per frame
        void Update()
        {

        }
        void CreateNewExclusionZone()
        {
            ExclusionZoneDrawer newExclusionZoneDrawwer = new ExclusionZoneDrawer();
            newExclusionZoneDrawwer.exclusionZoneMaterial = exclusionZoneMaterial;

        }
    }
