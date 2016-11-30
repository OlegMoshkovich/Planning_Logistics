using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class TagMenuManager : MonoBehaviour {

    // Use this for initialization
    private List<Tag> listOfQTrackTags = new List<Tag>();
    private List<GameObject> listOfDataRows = new List<GameObject>();
    public GameObject dataRow;
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
    }
    void LateUpdate()
    {
        
    }
    public void RefreshTagTable()
    {
        try
        {   
            listOfQTrackTags = mqttManager.main.listOfQTrackTags;
            for(int i=0; i<WorkerManager.main.listOfWorkers.Count; i++)
            {
                GameObject newRow = (GameObject) UnityEngine.Object.Instantiate(dataRow, GameObject.Find("TagTable").transform);
                newRow.transform.position = new Vector3(dataRow.transform.position.x, dataRow.transform.position.y - 40 * (i+1), dataRow.transform.position.z);
                newRow.transform.localScale = Vector3.one;
                newRow.GetComponentsInChildren<Text>()[0].text = WorkerManager.main.listOfWorkers[i].name;
                newRow.GetComponentsInChildren<Text>()[3].text = WorkerManager.main.listOfWorkers[i].GetComponent<WorkerTagMovement>().frequency.ToString();
                listOfDataRows.Add(newRow);
            }
        }
        catch (Exception error)
        {
            Debug.Log("error");
        }
    }
    public void EmptyTagTable()
    {
        foreach(GameObject dataRow in listOfDataRows)
        {
            Destroy(dataRow);
        }
        listOfDataRows = new List<GameObject>();
    }
    public void SubmitData()
    {
        
    }
}
