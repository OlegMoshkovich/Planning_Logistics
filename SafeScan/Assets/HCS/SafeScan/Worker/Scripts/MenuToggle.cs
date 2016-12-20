using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuToggle: MonoBehaviour {

	public GameObject CanvasObject;


	void OnMouseDown(){
        CanvasObject.SetActive(!CanvasObject.activeSelf);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.I)){
			CanvasObject.GetComponent<Canvas> ().enabled = !CanvasObject.GetComponent<Canvas> ().enabled;
		}	
	}



}
