using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class RandomMovement : MonoBehaviour {

    public int radius = 10;
    private NavMeshAgent agent;

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRandomGoal();
    }

    void Update () {
        //Update Animator speed
        if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetFloat("Speed", agent.speed);
        }
        //IF Agent reaches destination or has an invalid path, reset random goal
        if (agent.remainingDistance == 0 || agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial) SetRandomGoal();

    }

    void SetRandomGoal()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        Vector3 finalPosition = hit.position;
        agent.destination = finalPosition; 
    }
}
