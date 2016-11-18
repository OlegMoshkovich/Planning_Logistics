using UnityEngine;
using System.Collections;

namespace Safescan
{
    public class ExclusionZoneManager : MonoBehaviour
    {
        public Material exclusionZoneMaterial;

        // Use this for initialization
        void Start()
        {

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
}
