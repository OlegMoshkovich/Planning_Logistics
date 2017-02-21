using UnityEngine;
using System.Collections;

public class ObjectEnable: MonoBehaviour {

	bool toggleBool = true;
	public GameObject GO;

	void Update ()
	{
		if(Input.GetKeyUp(KeyCode.C))
		{
			toggleBool = !toggleBool;
			GO.SetActive(toggleBool);
				
		}
	}
}