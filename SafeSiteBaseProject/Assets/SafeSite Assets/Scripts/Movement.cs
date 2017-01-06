using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
	public float m_Speed = 12f;                 // How fast the object moves forward and back.
	public float m_TurnSpeed = 180f;            // How fast the object turns in degrees per second.
//
//	private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
//	private string m_TurnAxisName;              // The name of the input axis for turning.
////	private Rigidbody m_Rigidbody;              // Reference used to move the tank.
//
private float m_MovementInputValue;         // The current value of the movement input.
private float m_TurnInputValue;             // The current value of the turn input.
//

	public void Start(){
		m_MovementInputValue = Input.GetAxis ("JoystickVertical");
		m_TurnInputValue = Input.GetAxis ("JoystickHorizontal");
	
	}
		
	private void Update ()
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

//	
//		this.transform.position
	



		// Adjust the rigidbodies position and orientation in FixedUpdate.
//		Move ();
//		Turn ();
	}


//	private void Move ()
//	{

//		this.transform.position += Input.GetAxis("JoystickVertical") * transform.TransformDirection(Vector3.forward) + Input.GetAxis("JoystickHorizontal") * transform.TransformDirection(Vector3.right);
//		//	
//		// Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
//		Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
//	
//		// Apply this movement to the rigidbody's position.
//		this.transform.position = this.transform.position+movement;
////		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
		/// 


//	private void Turn ()
//	{
//		// Determine the number of degrees to be turned based on the input, speed and time between frames.
//		float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
//
//		// Make this into a rotation in the y axis.
//		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
//
//		// Apply this rotation to the rigidbody's rotation.
//		//m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
//		this.transform.rotation = this.transform.rotation*turnRotation;
//	}


}