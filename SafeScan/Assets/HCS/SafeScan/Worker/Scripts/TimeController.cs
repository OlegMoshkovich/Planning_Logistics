using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	


		for(int i = 1;i<10;i++)
		if(Input.GetKeyDown(i+""))
		Time.timeScale = (i);
	}
}
