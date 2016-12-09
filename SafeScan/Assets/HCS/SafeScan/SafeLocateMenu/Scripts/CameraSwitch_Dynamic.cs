using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CameraSwitch_Dynamic : MonoBehaviour {

	public Dropdown dropdown;
	public List<Camera> listOfCameras = new List<Camera>();
	private Camera selectedCamera;

	// Use this for initialization
	void Start() {
		populateCameraDropDown ();

	}

	void populateCameraDropDown(){
		dropdown.AddOptions(listOfCameras.Select(i => i.name).ToList());
	}

	public void dropDown_IndexChange(int index){
		for (int i = 0; i < listOfCameras.Count - 1; i++) {
			listOfCameras [i].gameObject.SetActive (false);	
		}
		selectedCamera = listOfCameras[index];// get the index of the selected camera
		selectedCamera.gameObject.SetActive (true);	
	}

}
