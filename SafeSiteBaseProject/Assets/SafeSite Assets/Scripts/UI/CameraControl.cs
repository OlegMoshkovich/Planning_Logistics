using UnityEngine;

public class CameraControl : MonoBehaviour {
    public Texture2D cursorPan;
    public Texture2D cursorZoom;
    public bool isometric = true;
	
	// Update is called once per frame
	void Update () {
        
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            //PANNING ON RIGHT CLICK
            if (Input.GetMouseButton(2))
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
            if(Input.GetAxis("JoystickHorizontal") != 0 || Input.GetAxis("JoystickVertical") != 0)
            {
                CameraSwitch.main.followTarget = false;
                this.transform.position += Input.GetAxis("JoystickVertical") * transform.TransformDirection(Vector3.forward) + Input.GetAxis("JoystickHorizontal") * transform.TransformDirection(Vector3.right);
           
			}
        //Zoom on pinch & Pan on drag two fingers
        float zoomSpeed = 0.01f;
        float panningSpeed = 0.005f;
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            CameraSwitch.main.followTarget = false;
            this.transform.position += deltaMagnitudeDiff * transform.TransformDirection(Vector3.forward) * zoomSpeed * -1;
            /*//Zoom & Pan with two fingers
            if ((touchZero.deltaPosition + touchOne.deltaPosition).sqrMagnitude < touchZero.deltaPosition.sqrMagnitude) //Fingers move in oposite direction
            {
                this.transform.position += deltaMagnitudeDiff * transform.TransformDirection(Vector3.forward) * zoomSpeed * -1;
            }    
           else
            {
                Cursor.SetCursor(cursorPan, Vector2.zero, CursorMode.Auto);
                this.transform.position -= touchZero.deltaPosition.x * transform.TransformDirection(Vector3.right) * panningSpeed + touchZero.deltaPosition.y * transform.TransformDirection(Vector3.up) * panningSpeed;
            }*/

        }
        //PANNING with three fingers
        if (Input.touchCount == 3)
        {
            CameraSwitch.main.followTarget = false;
            Vector2 touchDelta = Input.touches[0].deltaPosition + Input.touches[1].deltaPosition + Input.touches[2].deltaPosition;
            this.transform.position -= touchDelta.x * transform.TransformDirection(Vector3.right) * panningSpeed + touchDelta.y * transform.TransformDirection(Vector3.up) * panningSpeed;

        }
        //Point camera using Mouse right button or dragging finger
        if (Input.GetMouseButton(1) && Input.touchCount==0)
            {
                CameraSwitch.main.followTarget = false;
                float dx = Input.GetAxis("Horizontal");
                float dy = Input.GetAxis("Vertical");
                float rotationX = transform.localEulerAngles.y + dx * 0.4f;
                float rotationY = transform.localEulerAngles.x - dy * 0.4f;
                Mathf.Clamp(rotationY, 0, 180);
                transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
        }
        float touchRotationSpeed = 0.1f;
        if (Input.touchCount == 1)
        {
            if(Input.touches[0].phase == TouchPhase.Moved)
            {
                CameraSwitch.main.followTarget = false;
                float dx = Input.touches[0].deltaPosition.x;
                float dy = Input.touches[0].deltaPosition.y;
                float rotationX = transform.localEulerAngles.y + dx * touchRotationSpeed;
                float rotationY = transform.localEulerAngles.x - dy * touchRotationSpeed;
                Mathf.Clamp(rotationY, 0, 180);
                transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
            }
        }
    
	}
}
