using UnityEngine;
using System.Collections;

[System.Serializable]
public class SyncedAsset : MonoBehaviour {

    public int ID;
    public string name;
    public string type;

    public float[] position;
    public float[] rotation;

    public bool alwaysStatic;
    public MovementType movement;

    

    // Use this for initialization
    void Start () {
        Debug.Log(JsonUtility.ToJson(this));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
public enum MovementType {
    Static, SetMovement, RTLS, Random
}
