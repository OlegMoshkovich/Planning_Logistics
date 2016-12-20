using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
    public bool inverse = false;
	// Update is called once per frame
	void Update () {
    		Vector3 activeCamera = Camera.main.transform.position;
            if(GetComponent<RectTransform>() !=null) this.GetComponent<RectTransform>().rotation = Quaternion.LookRotation(this.GetComponent<RectTransform>().position - activeCamera);
            else this.transform.rotation = Quaternion.LookRotation(this.transform.position - activeCamera);
    }
}
