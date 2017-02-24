using UnityEngine;

public class SetCanvasRenderCamera : MonoBehaviour {

	void Start () {
        GetComponent<Canvas>().worldCamera = Camera.main;
	}
	
}
