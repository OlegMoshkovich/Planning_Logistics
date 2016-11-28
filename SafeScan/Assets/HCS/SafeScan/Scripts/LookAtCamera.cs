using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if(CameraSwitch.main.targetTransform != this.transform) this.transform.LookAt(Camera.main.transform);
	}
}
