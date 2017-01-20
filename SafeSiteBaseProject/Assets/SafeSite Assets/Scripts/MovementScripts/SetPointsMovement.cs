using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class SetPointsMovement : MonoBehaviour {

    public List<GameObject> destinationPoints = new List<GameObject>();

    private int selectedDestinationPoint = 0;
    private NavMeshAgent agent;
    
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        //Add two default points when starting
        AddDestinationPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        AddDestinationPoint(new Vector3(transform.position.x+5, transform.position.y, transform.position.z));
        AddDestinationPoint(new Vector3(transform.position.x , transform.position.y, transform.position.z+5));
    }
	void AddDestinationPoint(Vector3 pointPosition)
    {
        //destinationPoints.Add(new Vector3(transform.position.x + 5, transform.position.y, transform.position.z));
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        go.GetComponent<Renderer>().material.color = Color.yellow;
        go.transform.position = pointPosition;
        BoxCollider collider = go.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        go.AddComponent<ClickAndDrag>();
        destinationPoints.Add(go);
    }
	
	void Update () {
        //Update Animator speed
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetFloat("Speed", agent.speed);
        }
        //If reaches destination, go to next point
        if (agent.remainingDistance == 0)
        {
            agent.destination = NextDestinationPoint();
        }
	}
    //Returns next Destination
    private Vector3 NextDestinationPoint()
    {
        selectedDestinationPoint++;
        if (selectedDestinationPoint == destinationPoints.Count) selectedDestinationPoint = 0;
        return destinationPoints[selectedDestinationPoint].transform.position;
    }
    //Remove Destination Points on Destroy
    private void OnDestroy()
    {
        foreach(GameObject go in destinationPoints)
        {
            Destroy(go);
        }
        destinationPoints = new List<GameObject>();
    }
}
