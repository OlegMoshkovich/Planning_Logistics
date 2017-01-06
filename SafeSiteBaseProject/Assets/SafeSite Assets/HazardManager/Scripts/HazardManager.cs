using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardManager : MonoBehaviour {
    public GameObject HazardMarkerPrefab;
    public List<GameObject> listOfHazards = new List<GameObject>();
    public GameObject Hazards;

    public static HazardManager main;
	// Use this for initialization
	void Start () {
        main = this;
        Hazards = new GameObject();
        Hazards.name = "Hazards";
        Hazards.AddComponent<Folder>();
        TreeViewManager.main.TreeView.Add(Hazards);
        
    }

}
