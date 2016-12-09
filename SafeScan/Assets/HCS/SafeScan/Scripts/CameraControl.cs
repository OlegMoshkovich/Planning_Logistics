using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    public Texture2D cursorPan;
    public Texture2D cursorZoom;
    public bool isometric = true;
	
	// Update is called once per frame
	void Update () {
        if (isometric)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            //PANNING ON RIGHT CLICK
            if (Input.GetMouseButton(1))
            {
                CameraSwitch.main.followTarget = false;
                Cursor.SetCursor(cursorPan, Vector2.zero, CursorMode.Auto);
                this.transform.position -= Input.GetAxis("Horizontal") * transform.TransformDirection(Vector3.right) + Input.GetAxis("Vertical") * transform.TransformDirection(Vector3.up);
            }

            //ZOOM ON SCROLL
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                Cursor.SetCursor(cursorZoom, Vector2.zero, CursorMode.Auto);
                CameraSwitch.main.followTarget = false;
                this.transform.position += Input.GetAxis("Mouse ScrollWheel") * transform.TransformDirection(Vector3.forward) + Input.GetAxis("Horizontal") * transform.TransformDirection(Vector3.right) + Input.GetAxis("Vertical") * transform.TransformDirection(Vector3.up);
            }
        }
      
		if (!isometric)
        {
            if(Input.GetAxis("JoystickHorizontal") != 0 || Input.GetAxis("JoystickVertical") != 0)
            {
                CameraSwitch.main.followTarget = false;
                this.transform.position += Input.GetAxis("JoystickVertical") * transform.TransformDirection(Vector3.forward) + Input.GetAxis("JoystickHorizontal") * transform.TransformDirection(Vector3.right);
           
			}
            if (Input.GetMouseButton(1))
            {
                CameraSwitch.main.followTarget = false;
                float dx = Input.GetAxis("Horizontal");
                float dy = Input.GetAxis("Vertical");
                float rotationX = transform.localEulerAngles.y + dx * 0.4f;
                float rotationY = transform.localEulerAngles.x - dy * 0.4f;
                Mathf.Clamp(rotationY, 0, 180);
                transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
            }

        }
    
	}
}
