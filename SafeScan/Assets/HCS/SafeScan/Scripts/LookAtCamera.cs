using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
    public bool inverse = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

			Vector3 activeCamera = Camera.current.transform.position;	
			this.transform.rotation = Quaternion.LookRotation(this.transform.position - activeCamera);

        }
}
