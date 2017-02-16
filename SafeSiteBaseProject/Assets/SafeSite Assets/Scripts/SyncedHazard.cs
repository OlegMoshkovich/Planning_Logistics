using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class SyncedHazard : SyncedAsset {

    public Status status;
    public string description;
    public List<string> sa_imageURL;

    
    private RawImage imagePlaceholder;

    // Use this for initialization
    void Start () {
        //Debug.Log(JsonUtility.ToJson(this));       
	}
}
public enum Status { red, orange, green }
