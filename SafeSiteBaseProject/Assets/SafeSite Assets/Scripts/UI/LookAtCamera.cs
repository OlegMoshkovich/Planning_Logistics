using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    public bool inverse = false;

	void Update () {
    		Vector3 activeCamera = Camera.main.transform.position;
            if(GetComponent<RectTransform>() !=null) this.GetComponent<RectTransform>().rotation = Quaternion.LookRotation(this.GetComponent<RectTransform>().position - activeCamera);
            else this.transform.rotation = Quaternion.LookRotation(this.transform.position - activeCamera);
    }
}
