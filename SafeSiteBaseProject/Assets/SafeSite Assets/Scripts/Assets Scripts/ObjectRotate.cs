using UnityEngine;



//Allows to click rotate object by clicking on it and pressing 'r '
//For alternative with gizmo download Runtimetools by Battlehub//


public class ObjectRotate : MonoBehaviour {

	private bool drawerActive = false;
	void OnMouseDown()
	{
		if (drawerActive) {
			transform.Rotate(0, 90, 0);
			drawerActive = false;
		}
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("r")){
            drawerActive = !drawerActive;
			Debug.Log ("Rotation Drawer is :" + drawerActive);
		}
	}
}
