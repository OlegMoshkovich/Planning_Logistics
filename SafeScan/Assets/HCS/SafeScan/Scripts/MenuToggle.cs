using UnityEngine;
using System.Collections;

public class MenuToggle: MonoBehaviour {

	public GameObject CanvasObject;

	void Star(){

	}

	void OnMouseDown(){
		CanvasObject.GetComponent<Canvas> ().enabled = !CanvasObject.GetComponent<Canvas> ().enabled;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.I)){
			CanvasObject.GetComponent<Canvas> ().enabled = !CanvasObject.GetComponent<Canvas> ().enabled;
		}	
	}



}
