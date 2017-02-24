using UnityEngine;


public class ObjectMove : MonoBehaviour 
{

	private Vector3 screenPoint;

    protected void Start()
    {
        AssetManager.CheckCollider(this.gameObject);
    }


    void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
	}

	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		transform.position = Camera.main.ScreenToWorldPoint(curScreenPoint);    }

}