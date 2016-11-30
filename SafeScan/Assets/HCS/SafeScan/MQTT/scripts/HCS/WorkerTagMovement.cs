using UnityEngine;
using System.Collections;

public class WorkerTagMovement : MonoBehaviour {
    public int frequency;
    private Tag tag;
    private GameObject target;
    // Use this for initialization
    void Start()
    {
        tag = new Tag();
        target = new GameObject();
        target.transform.parent = this.gameObject.transform;
        this.GetComponent<WorkerMovement>().navMeshGoal = target;
    }
	
	// Update is called once per frame
	void Update () {
        foreach (Tag selectedTag in mqttManager.main.listOfQTrackTags)
        {
            if (frequency == selectedTag.Frequency) tag = selectedTag;
        }
        if (tag != null) target.transform.position = new Vector3(tag.X, 0, tag.Y);
        else Debug.LogError("couldn't find tag data");
    }
}
