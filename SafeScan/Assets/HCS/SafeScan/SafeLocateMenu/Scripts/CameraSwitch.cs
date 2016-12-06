using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CameraSwitch : MonoBehaviour {

	public Dropdown dropdown;
	private float extraOffsetX = 1F;
	private float extraOffsetY = 1F;
	private float extraOffsetZ = 1F;


    private Camera selectedCamera;
    public List<Camera> listOfCameras = new List<Camera>();
    public float locationSmoothTime = 0.3F;
    public float rotateSmoothFactor = 2.0F;
    private Vector3 cameraVelocity = Vector3.zero;
    public bool followTarget = false;
    public Transform targetTransform;
    private Vector3 cameraOffset = new Vector3();

    public static CameraSwitch main;

    // Use this for initialization
    void Start() {
        main = this;
        selectedCamera = listOfCameras[0];
        targetTransform = selectedCamera.transform;
        listOfCameras.Select(c => c.enabled = false);
		populateCameraDropDown ();
	}

    void Update()
    {
        /*if(Input.GetMouseButton(1)==true || Input.GetAxis("Mouse ScrollWheel") != 0 || Input.GetAxis("JoystickHorizontal") != 0 || Input.GetAxis("JoystickVertical") != 0)
        {
            followTarget = false;
            Camera.main.GetComponent<CameraControl>().enabled = true;
        }*/
        if (followTarget)
        {
            updateCameraWithOffset(targetTransform);
            //updateCameraWithoutOffset(targetTransform);
            //FocusCameraOnGameObject(Camera.main, targetTransform.gameObject);
        }
        else
        {
            //updateCameraWithoutOffset(targetTransform);
            
        }
        //Debug.Log("Selected Camera: " + targetTransform.position);
    }
	 
    public void SetTarget(Transform transform)
    {
        followTarget = true;
        targetTransform = transform;
    }

    private void updateCameraWithOffset(Transform transform)
    {
        if(transform != null)
        {
            Bounds b = CalculateBounds(transform.gameObject);
            float frustrumHeight = b.size.y;
			Vector3 extraOffset = new Vector3 (extraOffsetX, extraOffsetY, extraOffsetZ);

            float distance = frustrumHeight * 0.5f / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            cameraOffset = transform.forward * -distance + transform.up * distance;

			Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, transform.position + cameraOffset + extraOffset, ref cameraVelocity, locationSmoothTime);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, transform.rotation, Time.deltaTime * rotateSmoothFactor);
        }
    }
    private Bounds CalculateBounds(GameObject go)
    {
        Bounds b = new Bounds(go.transform.position, Vector3.zero);
        Object[] rList = go.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer r in rList)
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }

    private void FocusCameraOnGameObject(Camera c, GameObject go)
    {
        Bounds b = CalculateBounds(go);
        Vector3 max = b.size;
        float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
        float dist = radius / (Mathf.Sin(c.fieldOfView * Mathf.Deg2Rad / 2f));
        Debug.Log("Radius = " + radius + " dist = " + dist);
        Vector3 pos = Random.onUnitSphere * dist + b.center;
        c.transform.position = pos;
        c.transform.LookAt(b.center);
    }

    private void updateCameraWithoutOffset(Transform transform)
    {
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, transform.position, ref cameraVelocity, locationSmoothTime);
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, transform.rotation, Time.deltaTime * rotateSmoothFactor);
    }

    private void populateCameraDropDown(){
        dropdown.AddOptions(listOfCameras.Select(i => i.name).ToList());
	}

	public void dropDown_IndexChange(int index){
        //TreeViewManager.main.TreeView.Unselect();
        selectedCamera = listOfCameras[index];
        targetTransform = selectedCamera.transform;
        this.SetTarget(targetTransform);
        Debug.Log(targetTransform.position);
        Camera.main.GetComponent<CameraControl>().isometric = true;
	}

}