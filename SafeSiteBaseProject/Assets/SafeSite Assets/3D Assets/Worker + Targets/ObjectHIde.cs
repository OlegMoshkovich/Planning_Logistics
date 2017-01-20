using UnityEngine;
using System.Collections;

public class ObjectHIde : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("h")) {
			MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
			render.enabled = false;
		}

		if (Input.GetKeyDown ("s")) {
			MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
			render.enabled = true;
		}

	
	}
}
