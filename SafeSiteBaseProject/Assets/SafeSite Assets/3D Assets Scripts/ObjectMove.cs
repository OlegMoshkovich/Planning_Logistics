using UnityEngine;
using System.Collections;


public class ObjectMove : MonoBehaviour 
{

	private Vector3 screenPoint;

    private void Start()
    {
        if ((GetComponent<CapsuleCollider>() == null) & (GetComponent<BoxCollider>() == null) & (GetComponent<MeshCollider>() == null)) this.gameObject.AddComponent<MeshCollider>();
    }

    //private Vector3 offset;

    void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		//offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

	}

	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint);
		transform.position = curPosition;

	}

}