using UnityEngine;
using System.Collections;

public class FocusCameraOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CameraManager.main.SetTarget(this.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
