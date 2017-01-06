using UnityEngine;
using System.Collections;

    public class ExclusionZone : MonoBehaviour
    {
        public delegate void ExclusionZoneAction (Collision collision);
        public static event ExclusionZoneAction OnExclusionZoneEnter;
        public static event ExclusionZoneAction OnExclusionZoneExit;

        public float percentOfDanger = 0.8f;
        public string shape; // "Circle", "rectangle", "Mesh "
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    public void OnCollisionEnter(Collision collision)
    {
        //Trigger Event if it has subscribers
        if (OnExclusionZoneEnter != null) OnExclusionZoneEnter(collision);
        //Call ExclusionZoneManagerHandler
        ExclusionZoneManager.main.OnExclusionZoneEnter_Handler(collision);
    }
    public void OnCollisionStay(Collision collision)
    {
        ExclusionZoneManager.main.OnExclusionZoneStay_Handler(collision);
    }
    public void OnCollisionExit(Collision collision)
    {
        //Trigger Event if it has subscribers
        if (OnExclusionZoneEnter != null) OnExclusionZoneExit(collision);
        //Call ExclusionZoneManagerHandler
        ExclusionZoneManager.main.OnExclusionZoneExit_Handler(collision);
    }
    
}


