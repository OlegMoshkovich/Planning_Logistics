using UnityEngine;
using System.Collections;


[System.Serializable]
public class SyncedAsset : MonoBehaviour {

    public string _id;
    public string _rev;

    public string name;
    public string type;

    public Vector3 position;
    public Quaternion rotation;

    public bool alwaysStatic;
    public MovementType movement;


    public void UpdateWithJSON(string s)
    {
        JsonUtility.FromJsonOverwrite(s, this);
        updateGameObjectFromParameters();
    }

    // Use this for initialization
    void Start () {
        //Debug.Log(JsonUtility.ToJson(this));
	}
	
	// Update is called once per frame
	void Update () {
        updateParametersFromGameObject();
    }

    private void updateParametersFromGameObject()
    {
        name = gameObject.name;
        position = transform.position;
        rotation = transform.rotation;
    }
    private void updateGameObjectFromParameters()
    {
        gameObject.name = name;
        transform.position = position;
        transform.rotation = rotation;
        if (this.gameObject.GetComponent<NavMeshAgent>() != null)
        {
            this.gameObject.GetComponent<NavMeshAgent>().Warp(transform.position);
        }
    }
}


public enum MovementType {
    Static, SetMovement, RTLS, Random
}
