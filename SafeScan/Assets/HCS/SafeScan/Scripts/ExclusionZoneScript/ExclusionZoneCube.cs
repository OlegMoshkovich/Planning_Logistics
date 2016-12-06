using UnityEngine;
using System.Collections;

public class ExclusionZoneCube : MonoBehaviour {

	private GameObject prefab;
	private GameObject drawObject;
	bool drawing = false;
	Vector3 mouseStartingPosition;
	Vector3 Center;
	Camera cam;
	private GameObject target;
	private GameObject targetMouse;
	private int counter = 1;



	void Start () {
        prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        prefab.GetComponent<Renderer>().material = ExclusionZoneManager.main.exclusionZoneDangerMaterial;
        PlanView ();
		Debug.Log ("in the start function");
	}
		
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z));
		Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
		Plane zeroPlane = new Plane(Vector3.up, Vector3.zero);
		float distance = 0;
		Cursor.visible = false;

		if (zeroPlane.Raycast(ray, out distance)){
			mouseStartingPosition = ray.GetPoint(distance);
			if(target== null) target = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			target.GetComponent<Renderer> ().material.color = Color.red;
			target.transform.position = mouseStartingPosition;

		}
			
		if (Input.GetMouseButtonDown (0)) {
			startDraw ();
			target.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);


		} else if (Input.GetMouseButtonUp (0)) {
			endDraw ();
			Destroy (target);

		} else if (drawing) {
			whileDrawing ();
			target.GetComponent<Renderer> ().material.color = Color.green;
	
		}
	}

	void startDraw(){
		drawObject = Instantiate (prefab, mouseStartingPosition, Quaternion.identity) as GameObject;

		drawing = true;
		Camera.main.transform.position = new Vector3 (mouseStartingPosition.x, 10F, mouseStartingPosition.z);
	}

	void endDraw(){
		drawing = false;
		this.gameObject.SetActive(false);
		Destroy (targetMouse);
		Cursor.visible = true;

	}

	void whileDrawing(){
		float mouseDistanceX = mouseStartingPosition.x - Input.mousePosition.x;
		float mouseDistanceZ = mouseStartingPosition.z - Input.mousePosition.z;
		drawObject.transform.localScale = new Vector3 ( mouseDistanceX/200, 1f, mouseDistanceZ/2);
		//var collider = drawObject.GetComponent<BoxCollider>();
		//collider.transform.localScale = new Vector3 ( mouseDistanceX/500, 1f, mouseDistanceZ/8);
	}

	public void PlanView() {
        CameraSwitch.main.SetTarget(CameraSwitch.main.listOfCameras[0].transform);
	}

	void mouseTar(){

		target.gameObject.SetActive(true);
		targetMouse.gameObject.SetActive(true);
		target.transform.position = new Vector3 (-100f, -100f, -100f);
		targetMouse.transform.position = new Vector3 (-100f, -100f, -100f);
	}
		


}