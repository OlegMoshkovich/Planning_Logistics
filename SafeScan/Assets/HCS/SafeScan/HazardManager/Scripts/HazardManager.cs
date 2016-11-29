using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardManager : MonoBehaviour {
    public GameObject HazardMarkerPrefab;
    public List<GameObject> listOfHazards = new List<GameObject>();
    
    public static HazardManager main;
	// Use this for initialization
	void Start () {
        main = this;
        
	}
	
	// Update is called once per frame
	void Update () {
            
	}
}
