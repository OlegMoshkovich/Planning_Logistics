using UnityEngine;
using System.Collections;

    public class ExclusionZone : MonoBehaviour
    {
        public Vector3[] points;
        public delegate void ExclusionZoneAction (Collision collision);
        public static event ExclusionZoneAction OnExclusionZoneEnter;
        public static event ExclusionZoneAction OnExclusionZoneExit;
        
        public float percentOfDanger = 0.8f;
        public bool alert = true;
        public string shape; // "Circle", "rectangle", "Mesh "
        public void ExportExclusionZone()
    {
        Debug.Log(JsonUtility.ToJson(this.gameObject.AddComponent<SyncedHazard>()));
        
    }
        // Use this for initialization
        void Start()
        {
        //Invoke("ExportExclusionZone", 2);
        }

        // Update is called once per frame
        void Update()
        {

        }
    public void OnTriggerEnter(Collider collider)
    {
        if (alert)
        {
            //Trigger Event if it has subscribers
            //if (OnExclusionZoneEnter != null) OnExclusionZoneEnter(collider);
            //Call ExclusionZoneManagerHandler
            ExclusionZoneManager.main.OnExclusionZoneEnter_Handler(collider);
        }
        
    }
    public void OnTriggerExit(Collider collider)
    {
        if (alert)
        {
            //Trigger Event if it has subscribers
            //if (OnExclusionZoneEnter != null) OnExclusionZoneEnter(collider);
            //Call ExclusionZoneManagerHandler
            ExclusionZoneManager.main.OnExclusionZoneExit_Handler(collider);
        }

    }
    public void OnCollisionStay(Collision collision)
    {
        ExclusionZoneManager.main.OnExclusionZoneStay_Handler(collision);
    }
    public void OnCollisionExit(Collision collision)
    {
        //Trigger Event if it has subscribers
        //if (OnExclusionZoneEnter != null) OnExclusionZoneExit(collision);
        //Call ExclusionZoneManagerHandler
        //ExclusionZoneManager.main.OnExclusionZoneExit_Handler(collision);
    }
    
}


