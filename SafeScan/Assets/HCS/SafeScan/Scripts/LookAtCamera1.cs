using UnityEngine;
using System.Collections;

public class LookAtCamera1: MonoBehaviour {

	void Update () {
		Vector3 activeCamera = Camera.current.transform.position;	
		this.transform.rotation = Quaternion.LookRotation(this.transform.position - activeCamera);
	}
}
