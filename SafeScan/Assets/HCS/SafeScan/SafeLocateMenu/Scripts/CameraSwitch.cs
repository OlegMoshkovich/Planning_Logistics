using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CameraSwitch : MonoBehaviour {

	public Dropdown dropdown;
    private Camera selectedCamera;
    public List<Camera> listOfCameras = new List<Camera>();
    public float locationSmoothTime = 0.3F;
    public float rotateSmoothFactor = 2.0F;
    private Vector3 targetLocation;
    private Quaternion targetRotation;
    private Vector3 cameraVelocity = Vector3.zero;
    private bool followTarget = false;
    private Transform targetTransform;
    private Vector3 cameraOffset = new Vector3();

    // Use this for initialization
    void Start() {
        selectedCamera = listOfCameras[0];
        listOfCameras.Select(c => c.enabled = false);
		populateCameraDropDown ();
	}

    void Update()
    {
        if (followTarget)
        {
            updateCameraWithOffset(targetTransform);
        }
        else
        {
            updateCameraWithoutOffset(selectedCamera.transform);
            
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
        Bounds bounds = new Bounds(transform.position, Vector3.one);
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        cameraOffset = new Vector3(0, bounds.size.y, -bounds.size.z);
        cameraOffset = transform.rotation * cameraOffset;
        Camera.main.transform.localPosition = Vector3.SmoothDamp(Camera.main.transform.position, transform.position + cameraOffset, ref cameraVelocity, locationSmoothTime);
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, transform.rotation, Time.deltaTime * rotateSmoothFactor);
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
        followTarget = false;
        selectedCamera = listOfCameras[index];
	}

}