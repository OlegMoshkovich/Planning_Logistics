using UnityEngine;
using System.Collections;

public class ExclusionZoneRect : MonoBehaviour {

	public GameObject prefab;
	GameObject drawObject;
	bool drawing = false;
	Vector3 mouseStartingPosition;
	Vector3 Center;
	Camera cam;

	void Start () {
		PlanView ();
	}
		
	void Update () {

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z));
		Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
		Plane zeroPlane = new Plane(Vector3.up, Vector3.zero);
		float distance = 0;

		if (zeroPlane.Raycast(ray, out distance)){
			// get the hit point:
			mouseStartingPosition = ray.GetPoint(distance);
		}
			
		if (Input.GetMouseButtonDown (0)) {
			startDraw ();
		} else if (Input.GetMouseButtonUp (0)) {
			endDraw ();
		} else if (drawing) {
			whileDrawing ();
		}
	
	}

	void startDraw(){
		Debug.Log ("start drawing");
		drawObject = Instantiate (prefab, mouseStartingPosition, Quaternion.identity) as GameObject;
		drawing = true;
		Camera.main.transform.position = new Vector3 (mouseStartingPosition.x, 10F, mouseStartingPosition.z);
	}

	void endDraw(){
		Debug.Log ("end drawing");
		drawing = false;
		this.enabled = false;
	}

	void whileDrawing(){
		
		float mouseDistance = Vector3.Distance (mouseStartingPosition, Input.mousePosition);
		Debug.Log ("mouseDistance:" + mouseDistance/500);
		drawObject.transform.localScale = new Vector3 (mouseDistance/500, 1f, mouseDistance/500);

	}

	public void PlanView() {
		Camera[] listOfCameras = new Camera[10];
		listOfCameras = FindObjectsOfType<Camera>();
		Debug.Log (listOfCameras.Length);
		for (int i = 0; i < listOfCameras.Length; i++){
			if (listOfCameras [i].GetComponent<Camera> ().name == "Plan") {
				listOfCameras [i].GetComponent<Camera> ().enabled = true;

			} else {
				listOfCameras [i].GetComponent<Camera> ().enabled = false;

			}
		};
	}





}