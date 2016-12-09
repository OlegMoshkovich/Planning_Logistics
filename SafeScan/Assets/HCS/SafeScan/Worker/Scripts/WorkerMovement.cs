using UnityEngine;
using System.Collections;

public class WorkerMovement : MonoBehaviour {

    // Use this for initialization

	public Rigidbody rigidbody;
	public float speed = 2;

	public Rigidbody[] boneBodies;
	public CapsuleCollider coll;
	public Animator animator;
	public Rigidbody hips;

	public bool startWalking = false;

	public NavMeshAgent agent;
	public GameObject navMeshGoal;

	public Vector3 lastPos = new Vector3();
	public Vector3 velo = new Vector3();

	public GameObject dangerIndicator;
	public GameObject workerIndicator;
	public GameObject fallIndicator;

	public GameObject[] highlightParts;
	public Material defaultMaterial;
	public Material highlightMaterial;
	public Material dangerMaterial;
	public Material warningMaterial;

	public TrailRenderer trailRenderer;

	public int currentFloor = 0;

	public bool menuOpen = false;

	public Vector3 vel;
	public float speedLerpMultiplier = .8f;
	public bool adjustSpeed = true;
	public bool hitSomething = false;
	public bool ragged = false;

	public float walkRadius = 20;

	public bool randomWalk = false;
	public float randomWalkTimeMin = 2.0f;
	public float randomWalkTimeMax = 20.0f;

	public bool straightWalk = false;

	public Vector3[] positionArray;
	public int positionIndex = 0;

	public bool postArrived = false;

	public bool shouldRagdoll = false;

	public float lastMarkerTime;

	//public bool makePinIcon;
	//public GameObject pinIconPrefab;
	//public Transform pinIconTarget;

    private WorkerManager workerManager;

	void Start () {
        if (GameObject.FindObjectOfType<WorkerManager>() != null) workerManager = GameObject.FindObjectOfType<WorkerManager>();
        else Debug.Log("You are missing Worker Manager");
	rigidbody = GetComponent<Rigidbody>();




	/*if(makePinIcon)
	{
		
		GameObject pi = Instantiate(pinIconPrefab);

		if(pi!=null)
		pi.GetComponent<PinIcon>().target = pinIconTarget;

	}*/


	animator.SetFloat("Speed",0);

	if(navMeshGoal!=null)
	{
		agent.destination = navMeshGoal.transform.position;

		animator.SetFloat("Speed",speed);

		InvokeRepeating("UpdateNavGoal",.1f,.1f);
	}
	else if(randomWalk)
	{
		//Invoke("RandomNavGoal",1);
	}
	else
	{
		agent.enabled = false;
	}

	InvokeRepeating("CheckFloor",1,1);

	// Rag();

	if(randomWalk)
	{
		rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
	}
	else
	{
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
	}



	}

	public void RandomNavGoal()
	{
		if(!ragged)
		{

		if(startWalking)
		{
		hitSomething = false;
		//startWalking = true;
		Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

		randomDirection += transform.position;
		NavMeshHit hit;
 		NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
 		Vector3 finalPosition = hit.position;

 		agent.destination = finalPosition;

 		float dist = Vector3.Distance(transform.position,agent.destination);
		if(adjustSpeed)
		{

		if(speed>5)
		speed = 5;

		agent.speed = speed;
		//animator.SetFloat("Speed",agent.speed);
		}

		}
 		Invoke("RandomNavGoal", Random.Range(randomWalkTimeMin, randomWalkTimeMax));
 		}
	}

	public void UpdateNavGoal()
	{
		agent.destination = navMeshGoal.transform.position;
		float dist = Vector3.Distance(transform.position,agent.destination);
		if(adjustSpeed)
		agent.speed = dist*speedLerpMultiplier;

		if(agent.speed>5)
		agent.speed = 5;
		//animator.SetFloat("Speed",agent.speed);
	}

	public void CheckFloor()
	{
        Debug.Log("CheckFloor");
		RaycastHit hit;
        //float distanceToGround = 0;
        
        if (Physics.Raycast(transform.position+new Vector3(0,1,0), -Vector3.up, out hit, 2))
       {
       	//print("Hit something"+hit.collider.gameObject.name);
       		if(hit.collider.tag == "Danger")
       		{
       			currentFloor = 2;
       			HighlightDanger(true);
       		}
       		else if(hit.collider.tag == "Warning")
       		{
       			currentFloor = 1;

       			HighlightWarning(true);
       		}
            else if (hit.collider.GetComponent<ExclusionZone>() != null)
            {
                ExclusionZoneManager.main.OnWorkerHitExclusionZone(hit.collider);
            }
       		else
       		{
       			currentFloor = 0;
       			HighlightWarning(false);
       		}
       }
	}
	
	public void HighlightDanger(bool turnOn)
	{
		if(turnOn)
		{
			for(int i = 0;i<highlightParts.Length;i++)
			{
				highlightParts[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial = dangerMaterial;
			}
		}
		else
		{
			for(int i = 0;i<highlightParts.Length;i++)
			{
				highlightParts[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial = defaultMaterial;
			}
		}
	}
	public void HighlightWarning(bool turnOn)
	{

		if(menuOpen)
		{


		}
		else if(turnOn)
		{
			for(int i = 0;i<highlightParts.Length;i++)
			{
				highlightParts[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial = warningMaterial;
			}
		}
		else
		{
			for(int i = 0;i<highlightParts.Length;i++)
			{
				highlightParts[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial = defaultMaterial;
			}
		}
	}
	public void Highlight(bool turnOn)
	{


		if(turnOn)
		{
			menuOpen = true;
			for(int i = 0;i<highlightParts.Length;i++)
			{
				highlightParts[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial = highlightMaterial;
			}
		}
		else
		{
			menuOpen = false;
			for(int i = 0;i<highlightParts.Length;i++)
			{
				highlightParts[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial = defaultMaterial;
			}
		}
	}

	bool ShouldSpawnMarkers()
	{
		return (Time.time-lastMarkerTime>2f);
	}

	private Vector3 GetMeanVector(Vector3[] positions)
 	{
     if (positions.Length == 0)
         return Vector3.zero;
     float x = 0f;
     float y = 0f;
     float z = 0f;
     foreach (Vector3 pos in positions)
     {
         x += pos.x;
         y += pos.y;
         z += pos.z;
     }
     return new Vector3(x / positions.Length, y / positions.Length, z / positions.Length);
 	}

 	void CheckFalls()
 	{

 		float checkDistance = 1.2f;
 		Debug.DrawRay(transform.position+transform.up+(transform.forward*.5f), -transform.up*checkDistance, Color.green);
		Debug.DrawRay(transform.position+transform.up+(transform.forward*.3f), -transform.up*checkDistance, Color.green);
			if (!Physics.Raycast(transform.position+transform.up+(transform.forward*.5f), -transform.up, checkDistance)&&
				!Physics.Raycast(transform.position+transform.up+(transform.forward*.3f), -transform.up, checkDistance)) 
			{
				if(ShouldSpawnMarkers())
        		{
				if(!hitSomething)
				{
				GameObject fi = Instantiate(fallIndicator, transform.position-new Vector3(0,0,0), Quaternion.identity, WorkerManager.main.fallIndicatorsParent.transform) as GameObject;
				fi.transform.localScale*=.5f;
                    workerManager.listOfFallIndicators.Add(fi);
				}

				lastMarkerTime = Time.time;
				}
				if(shouldRagdoll)
				Rag();
				else
				agent.destination = transform.position+(-transform.forward).normalized*1;

			}

			if (!Physics.Raycast(transform.position+transform.up+(transform.right*.5f), -transform.up, checkDistance)&&
				!Physics.Raycast(transform.position+transform.up+(transform.right*.3f), -transform.up, checkDistance)) 
			{
				if(ShouldSpawnMarkers())
        		{
				if(!hitSomething)
				{
				GameObject fi = Instantiate(fallIndicator, transform.position-new Vector3(0,0,0), Quaternion.identity, WorkerManager.main.fallIndicatorsParent.transform) as GameObject;
				fi.transform.localScale*=.5f;
                    workerManager.listOfFallIndicators.Add(fi);
				}
				lastMarkerTime = Time.time;
				}
				if(shouldRagdoll)
				Rag2(transform.right*500);
				else
				agent.destination = transform.position+(-transform.right).normalized*1;
			}

			if (!Physics.Raycast(transform.position+transform.up+(transform.right*-.5f), -transform.up, checkDistance)&&
				!Physics.Raycast(transform.position+transform.up+(transform.right*-.3f), -transform.up, checkDistance)) 
			{
				if(ShouldSpawnMarkers())
        		{
				if(!hitSomething)
				{
				GameObject fi = Instantiate(fallIndicator, transform.position-new Vector3(0,0,0), Quaternion.identity, WorkerManager.main.fallIndicatorsParent.transform) as GameObject;
				fi.transform.localScale*=.5f;
                    workerManager.listOfFallIndicators.Add(fi);
                }
				lastMarkerTime = Time.time;
				}
				if(shouldRagdoll)
				Rag2(transform.right*-500);
				else
				agent.destination = transform.position+(transform.right).normalized*1;
			}
 	}
	// Update is called once per frame
	void Update () {


		velo = (transform.position-lastPos)/Time.deltaTime;
		if(Input.GetKeyDown("space"))
		{
			if(randomWalk)
			{
			startWalking = !startWalking;
			if(!startWalking)
			{
			CancelInvoke("RandomNavGoal");
			agent.destination = transform.position;
			}
			else
			{
				postArrived = false;
				Invoke("RandomNavGoal",0);
			}

			}
			else if (straightWalk)
			{

			startWalking = !startWalking;
			if(startWalking)
			animator.SetFloat("Speed",2);
			else
			animator.SetFloat("Speed",0);
			}
		}

		if(Input.GetKeyDown("t"))
		{
			trailRenderer.enabled = !trailRenderer.enabled;
		}

		if(!ragged)
		CheckFalls();

		

		if(navMeshGoal==null&&!randomWalk)
		{
		if(startWalking)
		{
			// = rigidbody.velocity;
			vel = transform.TransformDirection(Vector3.forward)*speed;
			vel.y = rigidbody.velocity.y;
			rigidbody.velocity = vel;
			rigidbody.angularVelocity*=0;

			//animator.SetFloat("Speed",speed);

			// if(vel.y<-3)
			// {
			// 	if(!hitSomething)
			// 	Instantiate(fallIndicator, transform.position-new Vector3(0,0,0), Quaternion.identity);
			// 	Rag();
			// }

			

		}
		else
		{

			rigidbody.velocity *= 0;
			rigidbody.angularVelocity*=0;

			
		}
		//if(Time.timeSinceLevelLoad>2)
		//animator.SetFloat("Speed",rigidbody.velocity.magnitude);
		}
		else
		{
			if(startWalking)
			{
				//agent.speed
			}
			else
			{
			//agent.speed = 0;
			}
		}

		if(positionIndex>=positionArray.Length-1)
		positionIndex = 0;
		else
		positionIndex++;

		positionArray[positionIndex] = transform.position;

		if(randomWalk)
		{
			Vector3 veloo = (transform.position-GetMeanVector(positionArray));
		velo = veloo/Time.deltaTime;
		//agent.velocity*=0;
		animator.SetFloat("Speed",velo.magnitude);

			if(!startWalking)
			agent.destination = transform.position;

			//if(Time.time>5)
			//CheckArrived();
		

		if(postArrived)
		{
			//postArrived = true;
			agent.enabled = false;
			randomWalk = false;
			navMeshGoal = null;

			straightWalk = true;
			
			Start();
			Invoke("ReRand", 1.5f);
		}

		}
		else
		{
		
		Vector3 veloo = (transform.position-GetMeanVector(positionArray));
		velo = veloo/Time.deltaTime;
		//agent.velocity*=0;
		animator.SetFloat("Speed",velo.magnitude);

		}
		
		//print(velo.magnitude);
		lastPos = transform.position;



	
	}

	void ReRand()
	{

		postArrived = false;
		agent.enabled = true;
		randomWalk = true;
		//navMeshGoal = null;

		straightWalk = false;
		
		Start();

		CancelInvoke("RandomNavGoal");
		RandomNavGoal();
	}

	void CheckArrived()
	{

		if(startWalking)
		{
		if (!agent.pathPending)
		 {
		     if (agent.remainingDistance <= agent.stoppingDistance)
		     {
		         if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
		         {
		             postArrived = true;
		         }
		     }
		 }
		}
	}

	void OnCollisionEnter(Collision collision) {
        
        if(collision.gameObject.tag == "Rag")
        {

            if (!hitSomething) {
                GameObject fi = Instantiate(fallIndicator, collision.contacts[0].point, Quaternion.identity, WorkerManager.main.fallIndicatorsParent.transform) as GameObject;
                workerManager.listOfFallIndicators.Add(fi);
            }
                
            hitSomething = true;

        Rag();

    	}

//    	print(collision.gameObject.tag);

    	if(collision.gameObject.tag == "Worker")
        {
        	

        	if(ShouldSpawnMarkers())
        	{
        	GameObject hg = Instantiate(workerIndicator, collision.contacts[0].point, Quaternion.identity, WorkerManager.main.fallIndicatorsParent.transform) as GameObject;
        	hg.transform.localScale*=.5f;
                workerManager.listOfWorkerIndicators.Add(hg);
            lastMarkerTime = Time.time;
        	}

        	agent.destination = transform.position+(transform.position-collision.contacts[0].point).normalized*1;
        }

    	if(collision.gameObject.tag == "Hit")
        {
        
        
        if(ShouldSpawnMarkers())
        	{
        if(!hitSomething)
        {
        GameObject hg = Instantiate(dangerIndicator, collision.contacts[0].point, Quaternion.identity, WorkerManager.main.dangerIndicatorsParent.transform) as GameObject;
        hg.transform.localScale*=.5f;
                    workerManager.listOfDangerIndicators.Add(hg);
        lastMarkerTime = Time.time;
    	}

    	}

    	agent.destination = transform.position;
        
        rigidbody.velocity *= 0;
		rigidbody.angularVelocity*=0;

		if(!randomWalk)
		{
		animator.SetTrigger("Hit");
		startWalking = false;
		}

		 rigidbody.velocity *= 0;
		rigidbody.angularVelocity*=0;
		//
		hitSomething = true;

		agent.destination = transform.position;
		CancelInvoke("UpdateNavGoal");

		agent.destination = transform.position+(transform.position-collision.contacts[0].point).normalized*2;
		CancelInvoke("RandomNavGoal");
		Invoke("RandomNavGoal",1);
		//agent.speed = 0;
		//animator.SetFloat("Speed",0);

    	}
    }

	void Rag()
	{


		coll.enabled = false;
		rigidbody.isKinematic = true;
		animator.enabled = false;
		hitSomething = true;

		
		if(agent)
		agent.speed = 0;
		for(int i = 0;i<boneBodies.Length;i++)
		{
			boneBodies[i].isKinematic = false;
			boneBodies[i].gameObject.GetComponent<Collider>().enabled = true;
			//boneBodies[i].gameObject.tag = "Hit";
		}

		//hips.AddForce(rigidbody.velocity*1000);
		if(!ragged)
		Invoke("RagForce", .01f);

		ragged = true;
	}

	void Rag2(Vector3 force)
	{
		
		Rag();
		RagForce(force);
		ragged = true;
	}

	void Stand()
	{
		for(int i = 0;i<boneBodies.Length;i++)
		{
			boneBodies[i].isKinematic = true;
			boneBodies[i].gameObject.GetComponent<Collider>().enabled = false;
			//boneBodies[i].gameObject.tag = "Hit";
		}
	}

	void RagForce(Vector3 force)
	{
		hips.AddForce(force);
	}
	void RagForce()
	{
//		print("asdkjhaskjdh");

		//hips.velocity = transform.TransformDirection(Vector3.forward)*speed*10;
		hips.AddForce(transform.TransformDirection(Vector3.forward)*500);//*900);
		//hips.AddForce(Vector3.up*200);
	}
}
