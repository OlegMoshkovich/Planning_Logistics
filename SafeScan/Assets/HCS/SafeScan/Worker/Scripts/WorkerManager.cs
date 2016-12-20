using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Battlehub.UIControls;

public class WorkerManager : MonoBehaviour {
    public List<GameObject> listOfWorkers = new List<GameObject>();
    public List<GameObject> listOfFallIndicators = new List<GameObject>();
    public List<GameObject> listOfDangerIndicators = new List<GameObject>();
    public List<GameObject> listOfWorkerIndicators = new List<GameObject>();
    public GameObject WorkerPrefab;

    public static WorkerManager main;

    public GameObject indicatorsParent;
    public GameObject workers;

    // Use this for initialization
    public delegate void ListAction();
    public static event ListAction OnChange;
    private void Awake()
    {
        main = this;
        indicatorsParent = new GameObject();
        indicatorsParent.name = "Safescan indicators";
        indicatorsParent.AddComponent<Folder>();
        workers = new GameObject();
        workers.name = "Workers";
        workers.AddComponent<Folder>();
    }
    void Start () {
       
        
        foreach (WorkerMovement wm in GameObject.FindObjectsOfType<WorkerMovement>())
        {
            listOfWorkers.Add(wm.gameObject);
        }
        
	}

	
	// Update is called once per frame
	void Update () {
        //Check for deletes
        for(int i=0; i<listOfWorkers.Count; i++)
        {
            if (listOfWorkers[i] == null) listOfWorkers.RemoveAt(i);
        }

	}
    public void CreateWorker()
    {
        GameObject newWorker = Instantiate(WorkerPrefab, workers.transform) as GameObject;
        listOfWorkers.Add(newWorker);
        TreeViewManager.main.TreeView.AddChild(workers, newWorker);
    }
}
