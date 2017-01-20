using UnityEngine;
using System.Collections;

public class ObjectMovementSimple : MonoBehaviour {

	// Use this for initialization

	public Rigidbody rigidbody;

	public Animator animator;

	public NavMeshAgent agent;
	public GameObject navMeshGoal;

	public TrailRenderer trailRenderer;
	public Transform[] points;
	private int destPoint = 0;



	void Start () {
		//animator.SetFloat("Speed",speed);
		agent = GetComponent<NavMeshAgent>();

		GotoNextPoint();
	}


	void GotoNextPoint() {
		// Returns if no points have been set up
		if (points.Length == 0)
			return;
		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		destPoint = (destPoint + 1) % points.Length;
	}


	void Update () {
		// Choose the next destination point when the agent gets
		// close to the current one.
		//agent.destination = navMeshGoal.transform.position;

		if (agent.remainingDistance < 0.5f)
			GotoNextPoint();
	}

}