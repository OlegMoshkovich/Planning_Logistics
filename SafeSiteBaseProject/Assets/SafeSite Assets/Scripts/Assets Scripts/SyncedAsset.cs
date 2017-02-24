using UnityEngine;


[System.Serializable]
public class SyncedAsset : MonoBehaviour {

    public string _id;
    public string _rev;

    public string sa_name;
    public string sa_type;
    public string sa_timeCreated;
    public string sa_timeChanged;
    public string sa_createdBy;
    public string sa_changedBy;

    public Vector3 sa_position;
    public Quaternion sa_rotation;

    public bool sa_alwaysStatic;
    public MovementType sa_movement;


    public void UpdateWithJSON(string s)
    {
        JsonUtility.FromJsonOverwrite(s, this);
        updateGameObjectFromParameters();
    }
    public static string GetUTCTimeStamp()
    {
        return System.DateTime.UtcNow.ToString("yyyyMMddHHmmss");
    }
    // Use this for initialization
    void Start () {
        if (sa_timeCreated != null) sa_timeCreated = GetUTCTimeStamp();
        //Debug.Log(JsonUtility.ToJson(this));
    }
	
	void Update () {
        updateParametersFromGameObject();
    }

    private void updateParametersFromGameObject()
    {
        sa_name = gameObject.name;
        sa_position = transform.position;
        sa_rotation = transform.rotation;

        sa_timeChanged = GetUTCTimeStamp();
       
    }
    private void updateGameObjectFromParameters()
    {
        gameObject.name = sa_name;
        transform.position = sa_position;
        transform.rotation = sa_rotation;
        var navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.Warp(transform.position);
        }

        if (gameObject.GetComponent<SyncedExclusionZone>() != null) gameObject.GetComponent<SyncedExclusionZone>().updateMesh();
    }
}


public enum MovementType {
    Static, SetMovement, RTLS, Random
}

public enum AssetType
{
    Bobcat, CementTruck, Fence, Forklift, Hazard, ExclusionZone, Ladder, SafetyNet, Scaffold, SidewalkShed, Worker, Other
}
