using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CameraSwitch : MonoBehaviour {

	public Dropdown dropdown;
	public List<Camera> listOfCameras = new List<Camera>();
	public List<Camera> listOfCameras_Dynamic = new List<Camera>();
	public float locationSmoothTime = 0.3F;
	public float rotateSmoothFactor = 2.0F;
	public bool followTarget = false;

    public InputField cameraNameInputField;

	public Transform targetTransform;

	public static CameraSwitch main;

    private Camera selectedCamera;
	private Vector3 cameraVelocity = Vector3.zero;
	private Vector3 cameraOffset = new Vector3 ();
  

    // Use this for initialization
    void Start() {
        main = this;
        selectedCamera = listOfCameras[0];
        targetTransform = selectedCamera.transform;
        listOfCameras.Select(c => c.enabled = false);
		populateCameraDropDown ();
	}

	private void populateCameraDropDown(){
        Debug.Log("PopulateCamera");
        if (dropdown != null)
        {
            Debug.Log("PopulateCamera not null");
            dropdown.ClearOptions();
            dropdown.AddOptions(listOfCameras.Select(i => i.name).ToList());
        }
	}


	public void dropDown_IndexChange(int index){//this function is tirggered by the dropdown change

		for (int i = 0; i < listOfCameras.Count - 1; i++) {
			listOfCameras [i].gameObject.SetActive (false);	
		}
		Camera.main.gameObject.SetActive (true);

		selectedCamera = listOfCameras[index];// get the index of the selected camera
		targetTransform = selectedCamera.transform;// get the transform of the selected camera
		this.SetTarget(targetTransform);//pass the transform of the selected camera to the SetTarget Function
	
	}

	public void SetTarget(Transform transform) //pass the selected camera transform to the function
	{
		followTarget = true; // when the function is called trigger the followTarget Switch of the Manager
		targetTransform = transform;//set the public targetTransform variable to the selected camera transorm
	}



    void Update()// watch for change of the follow Target to true
    {
        if (followTarget) // if follow target switch is triggered
        {
            updateCameraWithOffset(targetTransform);//Update the Main camera with the Transform of the selected Camera / Target
        }


    }
		
    private void updateCameraWithOffset(Transform transform)
    {
        if(transform != null)// check for the valifity of the transform variabl
        {
            Bounds b = CalculateBounds(transform.gameObject); // Calculate the bounding box of the target GameObject

            float frustrumHeight = b.size.y;

            float distance = frustrumHeight * 0.5f / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            cameraOffset = transform.forward * (-distance-1) + transform.up * distance;

			Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, transform.position + cameraOffset, ref cameraVelocity, locationSmoothTime);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, transform.rotation, Time.deltaTime * rotateSmoothFactor);
        }
    }

    private Bounds CalculateBounds(GameObject go)
    {
        Bounds b = new Bounds(go.transform.position, Vector3.zero);// Bounding Box - axis aligned - fully enclosed - the method accepts Center + Size
        Object[] rList = go.GetComponentsInChildren(typeof(Renderer));// get all the of the renderer components attacehd to the current target ???
       
		foreach (Renderer r in rList)// clarify?
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }
    public void RecordCameraPosition()
    {
        GameObject newCamera = new GameObject();
        newCamera.name = cameraNameInputField.text;
        newCamera.SetActive(false);
        Camera newCamera_CameraComponent = newCamera.AddComponent<Camera>();
        //Add properties of current camera
        newCamera.transform.position = Camera.main.transform.position;
        newCamera.transform.rotation = Camera.main.transform.rotation;
        newCamera_CameraComponent = Camera.main;
        listOfCameras.Add(newCamera.GetComponent<Camera>());
        populateCameraDropDown();
    }
    public void On2D3DButtonClick_Handler()
    {
        bool isometric = Camera.main.GetComponent<CameraControl>().isometric;
        Camera.main.GetComponent<CameraControl>().isometric = !isometric;
    }



//    private void FocusCameraOnGameObject(Camera c, GameObject go)
//    {
//        Bounds b = CalculateBounds(go);
//        Vector3 max = b.size;
//        float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
//        float dist = radius / (Mathf.Sin(c.fieldOfView * Mathf.Deg2Rad / 2f));
//        Debug.Log("Radius = " + radius + " dist = " + dist);
//        Vector3 pos = Random.onUnitSphere * dist + b.center;
//        c.transform.position = pos;
//        c.transform.LookAt(b.center);
//    }
		



}