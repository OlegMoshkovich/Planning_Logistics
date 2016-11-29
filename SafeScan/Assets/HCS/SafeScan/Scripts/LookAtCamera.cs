using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
    public bool inverse = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (CameraSwitch.main.targetTransform != this.transform)
        {
            if (!inverse) this.transform.LookAt(Camera.main.transform.position);
            if (inverse) this.transform.LookAt(-1* Camera.main.transform.position);
        }
        }
}
