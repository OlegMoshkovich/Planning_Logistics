using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PinIcon : MonoBehaviour {

	public Transform target;
	public Vector3 transformOffset;
	public Camera cam;
	public Image image;
	public Color color;
	public float distanceAlphaMultiplier;
	public float distanceStartPoint;

	public static PinIcon main;
	private string hideShortcut = "j";
	public static bool showIcon = true;
	// Use this for initialization
	void Start () {
	
		if(main==null)
		main = this;

		if(transform.parent==null)
		{
			if(GameObject.Find("Canvas")!=null)
			transform.parent = GameObject.Find("Canvas").transform;
			if(transform.parent == null)
			if(GameObject.Find("UICanvas")!=null)
			transform.parent = GameObject.Find("UICanvas").transform;

			if(transform.parent == null)
			{
				print("NO CANVAS");
				Destroy(gameObject);
			}


		}
		if(cam==null)
		{
			cam = Camera.main;
		}
	}

	void Update()
	{

		if(main == this)
		{
			if(Input.GetKeyDown(hideShortcut))
			{	
				showIcon = !showIcon;
			}
		 }
	}
	
	// Update is called once per frame
	void LateUpdate () {


		Vector3 forward = cam.transform.forward;
		Vector3 toOther = target.transform.position - cam.transform.position;
            
        if (Vector3.Dot(forward, toOther) > 0&&showIcon)
           {


		Vector3 screenPos = cam.WorldToScreenPoint(target.position+transformOffset);
		transform.position = screenPos;

		float distance = Vector3.Distance(cam.transform.position, target.transform.position);
		image.color = new Color(color.r,color.g,color.b,(distance*distanceAlphaMultiplier)-distanceStartPoint);
		}
		else
		{
			image.color = new Color(0,0,0,0);
		}
	}
}
