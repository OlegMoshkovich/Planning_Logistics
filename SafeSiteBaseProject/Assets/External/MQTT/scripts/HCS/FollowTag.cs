using UnityEngine;
using System;
using System.Collections;

public class FollowTag : MonoBehaviour {
    public int frequency;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        Tag tag = new Tag();
        try
        {
            tag = mqttManager.main.listOfQTrackTags[frequency.ToString()];
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        Vector3 target = new Vector3(tag.X, 0, tag.Y);
        if (tag != null) this.transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }
}
