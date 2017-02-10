using UnityEngine;
using System.Collections;

[System.Serializable]
public class SyncedHazard : SyncedAsset {

    public Status status;
    public string description;
    public byte[] image;

	// Use this for initialization
	void Start () {
        Debug.Log(JsonUtility.ToJson(this));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
public enum Status { red, orange, green }
