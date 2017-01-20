using UnityEngine;
using System.Collections;

[System.Serializable]
public class SyncedHazard : SyncedAsset {

    public Status status;
    public string description;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
public enum Status { red, orange, green }
