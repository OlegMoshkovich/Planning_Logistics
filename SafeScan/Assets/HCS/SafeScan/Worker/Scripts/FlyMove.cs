using UnityEngine;
using System.Collections;

public class FlyMove : MonoBehaviour {

	// Use this for initialization


	float defaultSpeed = .1f;


	static Vector3 pos;
	public static Vector3 rot;

	void Start () {
	
	if(pos!=Vector3.zero)
	{
	transform.position = pos;
	transform.eulerAngles = rot;
	}	
	
	}
	
	// Update is called once per frame
	void Update () {



		float speed = defaultSpeed;

		if(Input.GetKey(KeyCode.LeftShift))
		{
		speed*=2f;
		//print("sdlkfjsdf "+speed);
		}
		transform.position+=((Input.GetAxis("Vertical")*transform.TransformDirection(Vector3.forward))+(Input.GetAxis("Horizontal")*transform.TransformDirection(Vector3.right))+(Input.GetAxis("Vertical")*transform.TransformDirection(Vector3.up)))*speed*Time.deltaTime*60*(1/Time.timeScale);
	

		if(Input.GetKeyDown("p"))
		{
			pos = transform.position;
			rot = transform.eulerAngles;
			Application.LoadLevel(0);

		}
	}
}
