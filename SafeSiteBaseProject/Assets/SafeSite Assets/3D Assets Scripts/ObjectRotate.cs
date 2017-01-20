using UnityEngine;
using System.Collections;

public class ObjectRotate : MonoBehaviour {

	private bool drawerActive = false;
	void OnMouseDown()
	{
		Debug.Log ("mouse is pressed");
		if (drawerActive) {
			transform.Rotate(0, 0, 90);
			drawerActive = false;
		}


	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("r")){
			drawerActive = true;
			Debug.Log ("drawer is active");
		}

		if(Input.GetKeyDown("e")){
			drawerActive = false;
			Debug.Log ("drawer is siabled");
		}

	
	}
}
