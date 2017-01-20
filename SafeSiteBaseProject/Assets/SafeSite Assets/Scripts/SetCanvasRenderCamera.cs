using UnityEngine;
using System.Collections;

public class SetCanvasRenderCamera : MonoBehaviour {

	void Start () {
        GetComponent<Canvas>().worldCamera = Camera.main;
	}
	
}
