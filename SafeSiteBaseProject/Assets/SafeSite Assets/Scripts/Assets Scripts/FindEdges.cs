using UnityEngine;
using System.Collections;

public class FindEdges : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        CheckFalls();
	
	}
    void CheckFalls()
    {
        float checkDistance = 1.2f;
        Vector3 rayOrigin = transform.position + transform.up + (transform.forward * .5f);
        Vector3 rayDirection = -transform.up;

        Debug.DrawRay(rayOrigin, rayDirection, Color.green);
        if (!Physics.Raycast(rayOrigin, rayDirection, checkDistance))
        {
            HazardManager.main.AddEdge(rayOrigin + rayDirection * checkDistance);
        }
     }
 
}
