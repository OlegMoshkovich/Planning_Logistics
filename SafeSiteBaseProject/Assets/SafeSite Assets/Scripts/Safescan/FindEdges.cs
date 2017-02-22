using UnityEngine;
using System.Collections;

//Object raycats a certain distance and height to check for edges
public class FindEdges : MonoBehaviour {


    public float checkDistanceDown = 1.2f;
    public float raycastDistanceForward = .8f;
    public float secondsToRepeatCheck = 0.3f;


    protected void Start()
    {
        InvokeRepeating("CheckFalls", 0, secondsToRepeatCheck);
    }

    void CheckFalls()
    {
        Vector3 rayOrigin = transform.position + transform.up + (transform.forward * raycastDistanceForward);
        Vector3 rayDirection = -transform.up;

        Debug.DrawRay(rayOrigin, rayDirection, Color.green);
        if (!Physics.Raycast(rayOrigin, rayDirection, checkDistanceDown))
        {
            HazardManager.main.AddEdge(rayOrigin + rayDirection * checkDistanceDown);
        }
     }
 
}
