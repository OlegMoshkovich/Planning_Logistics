using UnityEngine;
using System.Collections;

public class ObjectDestroy : MonoBehaviour {

	// Use this for initialization

	private bool drawerActive = false;
	void OnMouseDown()
	{
		drawerActive = true;
		Debug.Log ("mouse is pressed");
	}
	
	// Update is called once per frame

	void Update () {


			if (drawerActive) {
				//transform.Rotate(0, 0, 90);
			if (Input.GetKeyDown("d")) {
					Destroy (gameObject);
				drawerActive = false;
				}

			}
		
	}

}