using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class SyncedHazard : SyncedAsset {

    public Status sh_status;
    public HazardType sh_type;
    public string sh_description;
    public List<string> sa_imageURL;

    
    private RawImage imagePlaceholder;

    // Use this for initialization
    void Start () {
        //Debug.Log(JsonUtility.ToJson(this));       
	}
}
public enum Status { red, orange, green }
public enum HazardType { Trip, Fall, CaughtInBetween, StruckBy, Other}
