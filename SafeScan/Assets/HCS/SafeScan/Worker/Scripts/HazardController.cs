using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardController : MonoBehaviour {


	public static HazardController main;
	public List<HazardMarker> dangerMarkers; 
	public List<HazardMarker> personMarkers;
	public List<HazardMarker> fallMarkers;

	public string transparentKey = "m";
	public string opaqueKey = "n";

	public string dangersKey = "v";
	public string personsKey = "c";
	public string fallsKey = "x";

	public bool showDangers = true;
	public bool showPersons = true;
	public bool showFalls = true;

	public int materialMode = 0;
	// Use this for initialization
	void Start () {
		main = this;

		dangerMarkers = new List<HazardMarker>();
		personMarkers = new List<HazardMarker>();
		fallMarkers = new List<HazardMarker>();
	}
	
	public void AddMarker(HazardMarker hm, int t)
	{
		if(t==0)
		{
			dangerMarkers.Add(hm);
		}
		else if (t == 1)
		{
			personMarkers.Add(hm);
		}
		else if (t == 2)
		{
			fallMarkers.Add(hm);
		}

		if(materialMode == 1)
		{
			hm.SetOpaque();
		}
	}
	// Update is called once per frame
	void Update () {


		if(Input.GetKeyDown(personsKey))
		{
			showPersons = !showPersons;
			foreach(HazardMarker hm in personMarkers)
			hm.gameObject.SetActive(showPersons);
		}

		if(Input.GetKeyDown(dangersKey))
		{
			showDangers = !showDangers;
			foreach(HazardMarker hm in dangerMarkers)
			hm.gameObject.SetActive(showDangers);
		}

		if(Input.GetKeyDown(fallsKey))
		{
			showFalls = !showFalls;
			foreach(HazardMarker hm in fallMarkers)
			hm.gameObject.SetActive(showFalls);
		}

		if(Input.GetKeyDown(transparentKey))
		{
			foreach(HazardMarker hm in dangerMarkers)
			hm.SetTransparent();
			foreach(HazardMarker hm in personMarkers)
			hm.SetTransparent();
			foreach(HazardMarker hm in fallMarkers)
			hm.SetTransparent();

			materialMode = 0;
		}

		if(Input.GetKeyDown(opaqueKey))
		{
			foreach(HazardMarker hm in dangerMarkers)
			hm.SetOpaque();
			foreach(HazardMarker hm in personMarkers)
			hm.SetOpaque();
			foreach(HazardMarker hm in fallMarkers)
			hm.SetOpaque();

			materialMode = 1;
		}
	
	}

}
