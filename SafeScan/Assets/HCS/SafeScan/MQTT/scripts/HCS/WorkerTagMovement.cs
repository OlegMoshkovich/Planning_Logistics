using UnityEngine;
using System.Collections;

public class WorkerTagMovement : MonoBehaviour {
    public int frequency;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Tag tag;
    // Use this for initialization
    void Start()
    {
       
	
	}
	
	// Update is called once per frame
	void Update () {
          Debug.Log("Velocity: "+ velocity);
        foreach (Tag selectedTag in mqttManager.main.listOfQTrackTags)
        {
            if (frequency == selectedTag.Frequency) tag = selectedTag;
        }
        Vector3 target = new Vector3(tag.X, 0, tag.Y);
        if (tag != null) this.transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        this.GetComponent<Animator>().SetFloat("Speed", velocity.magnitude);
        GetComponent<NavMeshAgent>().SetDestination(target);
    }
}
