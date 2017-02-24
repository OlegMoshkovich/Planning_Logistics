using UnityEngine;

//Object raycats a certain distance and height to check for edges
public class FindEdges : MonoBehaviour {

    public float checkDistanceDown = 1.8f;
    public float raycastDistanceForward = .8f;
    public float secondsToRepeatCheck = 0.3f;


    protected void Start()
    {
        //InvokeRepeating("CheckFalls", 0, secondsToRepeatCheck);
    }

    private void Update()
    {
        CheckFalls();
    }
    void CheckFalls()
    {
        Vector3 rayOrigin = gameObject.transform.position + gameObject.transform.up + (gameObject.transform.forward * raycastDistanceForward);
        Vector3 rayDirection = -gameObject.transform.up;
        Debug.DrawRay(rayOrigin, rayDirection, Color.green);
        if (!Physics.Raycast(rayOrigin, rayDirection, checkDistanceDown))
        {
            if (HazardManager.main != null) HazardManager.main.AddEdge(rayOrigin + rayDirection * checkDistanceDown);
            else Debug.Log("Missing HAzard Manager");
        }
     }
 
}
