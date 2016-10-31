using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook2 : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public static float mouseSensitivity = 1.0f;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;
	
	public Vector3 startPos;
	public Vector3 recoil;

	public Vector3 screenShake;
	public Transform myTransform;
	
	public int lean = 0;
	private float leanAngle;

	static public float rotationY;// = 0F;
	public bool smoothing = false;
	
	bool ai = false;
	public static bool inverted = false;

	public AnimationCurve  smoothCurve = AnimationCurve.Linear(0,0,1,1);

	public float smooth = 30F;

	public GameObject animCam;
	

	public bool lockedCameraAnimation = false;
	public bool semiLockedCameraAnimation = false;

	void OnEnable()
	{
		if(ai)
		this.enabled = false;	
	}
	
	void Ai()
	{
		ai = true;
		this.enabled = false;	
	}

	// Increase numberSamples to increase smoothness.
	const int numberSamples = 50;

	int nextSample = 0;
	Vector2[] inputSamples;

	// Use this for initialization
	void Awake () {
		inputSamples = new Vector2[numberSamples];
		for(int i = 0; i < numberSamples; i++) {
			inputSamples[i] = Vector2.zero;
		}
	}
	
	Vector2 Average() {
		Vector2 accumulative = Vector2.zero;

		for(int i = 0; i < numberSamples; i++) {
			accumulative += inputSamples[i]/numberSamples;
		}

		return accumulative;
	}

	void Update ()
	{
		
		// if(World.canKey)
		// {
		// if(Input.GetKeyDown("["))
		// {
		// 	mouseSensitivity-=.05f;
			
		// 	if(mouseSensitivity<0f)
		// 	mouseSensitivity = 0f;
		// }
		// if(Input.GetKeyDown("]"))
		// {
		// 	mouseSensitivity+=.05f;
			
		// 	if(mouseSensitivity>2.0f)
		// 	mouseSensitivity = 2.0f;
		// }
		
//		if(Input.GetKeyDown("'"))
//		{
//			smoothing = !smoothing;
//			if(smoothing)
//			mouseSensitivity = .3f;
//			else
//			mouseSensitivity = 1;
//		}
			
		// }
		
		//if(!World.menuOpen)
		mouseSensitivity = 1;//World.mouseSensitivity;
		//else
		//mouseSensitivity = .035f;

		//smoothing = World.mouseSmoothing;
		
		if(inverted&&sensitivityY>0)
		sensitivityY = -sensitivityY;

		if(!inverted&&sensitivityY<0)
		sensitivityY = -sensitivityY;


		
		float dx = Input.GetAxis("Mouse X");
		
		float dy = Input.GetAxis("Mouse Y");
		

		if(!Input.GetButton("Fire2"))
		{
			dx = 0;
			dy = 0;
		}
		
		

		if(smoothing)
		{



			Vector2 smoothed = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));// = Smoothify(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));

			inputSamples[nextSample] = smoothed;

		nextSample++;

		if(nextSample >= numberSamples) {
			nextSample = 0;
		}


		 smoothed = Average();

			

			dx = smoothed.x;
			dy = smoothed.y;
			// float threshold = 1f;
			// float invThrehsold = 1/threshold;

			// dx = ApplySmoothing(dx);//ApplySmoothing(Input.GetAxis("Mouse X")*invThrehsold)/invThrehsold;
			// dy = ApplySmoothing(dy);//ApplySmoothing(Input.GetAxis("Mouse Y")*invThrehsold)/invThrehsold;
			// if(Mathf.Abs(dx)<.2f)
			// dx = ApplySmoothing(Input.GetAxis("Mouse X")*5f)/5f;
			// if(Mathf.Abs(dy)<.2f)
			// dy = ApplySmoothing(Input.GetAxis("Mouse Y")*5f)/5f;
		// if(dx<1)
		// dx = (dx * Mathf.Abs(dx)*10f)/10f; // squares mouse x while keeping the polarity

		// // if(dy<1)
		// dy = (dy * Mathf.Abs(dy)*10f)/10f;
		
		// dx = Mathf.Clamp(dx,-5,5);
		// dy = Mathf.Clamp(dy,-5,5);

//			print(dx+"  "+dy);
		}
		
		
		
		
		//print(mouseSensitivity+"   "+smoothing+"   "+dx);
		if (axes == RotationAxes.MouseXAndY)
		{

			
			float rotationX = transform.localEulerAngles.y + dx * sensitivityX *mouseSensitivity;
			
			rotationY += dy * sensitivityY * mouseSensitivity;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			if(Input.GetButton("Fire2"))
			{
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else
			{
				if(transform.eulerAngles.x<270)
				rotationY = -transform.eulerAngles.x;
				else
				rotationY = 360-transform.eulerAngles.x;

				rotationX = transform.eulerAngles.y;
			}

		}
		else if(axes == RotationAxes.MouseY)
		{
			rotationY += dy * mouseSensitivity * sensitivityY+recoil.y;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			

			rotationY += screenShake.y;


			if(lean == 1)
	        {
	        leanAngle = Mathf.Lerp(leanAngle,-20,Time.deltaTime*3);	
	        	
	        }
	        if(lean == 2)
	        {
	        leanAngle = Mathf.Lerp(leanAngle,20,Time.deltaTime*3);	
	        	
	        }
	         if(lean == 0)
	        {
	        leanAngle = Mathf.Lerp(leanAngle,0,Time.deltaTime*3);	
	        	
	        }
			
			

			if(lockedCameraAnimation)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, animCam.transform.rotation, Time.deltaTime * 20);
				transform.localPosition = Vector3.Lerp(transform.localPosition, animCam.transform.localPosition, Time.deltaTime * 20);

				rotationY= 0;

			}
			else if(semiLockedCameraAnimation)
			{

				float semiRot = Mathf.Clamp((animCam.transform.localEulerAngles.x-rotationY),  minimumY, maximumY);
				Quaternion rot = Quaternion.Euler(semiRot, transform.localEulerAngles.y, leanAngle);

				transform.localPosition = Vector3.Lerp(transform.localPosition, animCam.transform.localPosition, Time.deltaTime * 20);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * 20); 
			
			}
			else
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, leanAngle);

			//transform.localEulerAngles = animCam.transform.localEulerAngles;//Vector3.Slerp(transform.localEulerAngles, animCam.transform.localEulerAngles, 20*Time.deltaTime);
			
//			float smooth2 = 30F;
//			Vector3 newPosition = transform.localPosition;
//			//newPosition.z += 0.1f;
//			
//			newPosition.z = Mathf.Lerp(newPosition.z,startPos.z, Time.deltaTime * smooth2);
//			
//			transform.localPosition = newPosition;
			
			//transform.position.z 
			
//			transform.Rotate(-Input.GetAxis("Mouse Y") * sensitivityY,0, 0);
//			
//			rotationY = Mathf.Clamp (transform.localEulerAngles.x, minimumY, maximumY);
//			
//			transform.localEulerAngles = new Vector3(-rotationY, 0 , 0);
			
		}
		else if (axes == RotationAxes.MouseX)
		{

			transform.Rotate(0, dx * sensitivityX*mouseSensitivity+Random.Range(-recoil.x,recoil.x)+screenShake.x, 0);
			
			//print(recoil.x);
		}
		
		//float smooth = 30F;

		//smooth = Mathf.Lerp(smooth,30f, Time.deltaTime * 2);

			recoil.y = Mathf.Lerp(recoil.y,0, Time.deltaTime * smooth);
			recoil.x = Mathf.Lerp(recoil.x,0, Time.deltaTime * smooth);

			screenShake.y = Mathf.Lerp(screenShake.y,0, Time.deltaTime * smooth);
			screenShake.x = Mathf.Lerp(screenShake.x,0, Time.deltaTime * smooth);
			
			
		//if(pm!=null)
		//{
		

	
			
		//}
			
			//myTransform.localPosition.z = Mathf.Lerp(myTransform.localPosition.z,startPos.z, Time.deltaTime * smooth);
	}
	
//	public void Recoil(float rec)
//	{
//		recoil = rec;
//		
//	}
	
	float ApplySmoothing(float finput)
	{
	  //if(Mathf.Abs(finput) < 1f) {

	  	float pos = 1f;
	  	if(finput<0)
	  	pos = -1f;

	    return smoothCurve.Evaluate(Mathf.Abs(finput))*pos; //finput * Mathf.Abs(finput);
	  //}
	  
	  //return finput;
	}


public static float threshold = 0.3f;

Vector2 Smoothify(Vector2 movement) 
{

	
  float invThreshold = 1f/threshold;

  float magnitude = movement.magnitude*invThreshold;
  float smoothMagnitude;

  if(magnitude<threshold*10)
  smoothMagnitude = smoothCurve.Evaluate(magnitude);
  else
  smoothMagnitude = magnitude;
  
  return movement.normalized*(smoothMagnitude/invThreshold);
}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
			
		myTransform = transform;
		startPos = transform.localPosition;

		//rotationY = FlyMove.rot.x;
	}
}