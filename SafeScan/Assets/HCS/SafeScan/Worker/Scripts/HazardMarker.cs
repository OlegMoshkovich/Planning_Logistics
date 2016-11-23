using UnityEngine;
using System.Collections;

public class HazardMarker : MonoBehaviour {


	public Material opaqueMaterial;
	public Material transparentMaterial;

	public int type = 0;

	// // Use this for initialization
	// void Start () {
	
	// }
	
	// // Update is called once per frame
	// void Update () {
	
	// }

	void Start()
	{
        if (HazardController.main != null)
        {
            HazardController.main.AddMarker(this, type);

            if (type == 0)
                gameObject.SetActive(HazardController.main.showDangers);

            if (type == 1)
                gameObject.SetActive(HazardController.main.showPersons);

            if (type == 2)
                gameObject.SetActive(HazardController.main.showFalls);
        }
        else Debug.Log("No Hazard controller found");
	}

	public void SetTransparent()
	{
		SetMaterial(transparentMaterial);
	}

	public void SetOpaque()
	{
		SetMaterial(opaqueMaterial);
	}

	void SetMaterial(Material mat)
	{

		if(GetComponent<Renderer>()!=null)
    		GetComponent<Renderer>().sharedMaterial = mat;
	}
}
