using UnityEngine;
using System.Collections;

public class TagRow : MonoBehaviour {
    public int index=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    public void OnQTrackTextSubmit(int value)
    {
        WorkerManager.main.listOfWorkers[index].GetComponent<WorkerTagMovement>().frequency = value;
    }
}
