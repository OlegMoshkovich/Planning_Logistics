using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {

	public Dropdown dropdown;
	private List<Camera> listOfCameras = new List<Camera>();
	public float locationSmoothTime = 0.3F;
	public float rotateSmoothFactor = 2.0F;
	public bool followTarget = false;

    public InputField cameraNameInputField;

	public Transform targetTransform;

	public static CameraManager main;

    private Camera selectedCamera;
	private Vector3 cameraVelocity = Vector3.zero;
	private Vector3 cameraOffset = new Vector3 ();
  

    // Use this for initialization
    void Start() {
        main = this;

        SetUpCameras();
	}

    public void SetUpCameras()
    {
        //Find all cameras and fill box
        listOfCameras.AddRange(FindObjectsOfType<Camera>().Where(c => c.name != "MainCamera").ToList<Camera>());
        if (listOfCameras.Count == 0) Debug.LogError("You must add at least one camera");
        for (int i = 0; i < listOfCameras.Count; i++)
        {
            listOfCameras[i].gameObject.SetActive(false);
        }
        populateCameraDropDown();
        dropDown_IndexChange(0);
    }
	private void populateCameraDropDown(){
        if (dropdown != null)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(listOfCameras.Select(i => i.name).ToList());
        }
	}

	public void dropDown_IndexChange(int index){//this function is tirggered by the dropdown change
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
        if(transform != null)// check for the valifity of the transform variable
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
        GameObject camera = (GameObject)Instantiate(Resources.Load<GameObject>("AssetsLibrary/Camera"), Camera.main.transform.position, Camera.main.transform.rotation);
        camera.name = cameraNameInputField.text;
        var sa = camera.GetComponent<SyncedAsset>();
        if (sa == null) Debug.LogError("Resource is missing synced asset");
        SetUpCameras();
    }


}