using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class TagMenuManager : MonoBehaviour {



    public Dictionary<GameObject, GameObject> tagRows  = new Dictionary<GameObject, GameObject>(); //<Worker, Tag Row>
    public GameObject dataRow;
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
    }
    void LateUpdate()
    {
        
    }
    public void ClearRows()
    {
        foreach(KeyValuePair<GameObject, GameObject> entry in tagRows)
        {
            Destroy(entry.Value);
        }
        tagRows.Clear();
    }
    public void RefreshTagTable()
    {
        //Clear rows
        ClearRows();
        //Populate Rows
        try
        {
            for (int i = 0; i < WorkerManager.main.listOfWorkers.Count; i++)
            {
                GameObject selectedRow = (GameObject)UnityEngine.Object.Instantiate(dataRow, this.transform);
                tagRows.Add(WorkerManager.main.listOfWorkers[i],selectedRow);
                selectedRow.transform.localScale = Vector3.one;
                selectedRow.transform.position = new Vector3(600, 400 - 40 * (i + 1), 0);
                selectedRow.GetComponentsInChildren<Text>()[0].text = WorkerManager.main.listOfWorkers[i].name;
                selectedRow.GetComponentsInChildren<Text>()[3].text = WorkerManager.main.listOfWorkers[i].GetComponent<WorkerTagMovement>().frequency.ToString();
            }
        
        }
        catch (Exception error)
        {
            Debug.Log(error.ToString());
        }
    }

    public void SubmitData()
    {
        foreach (KeyValuePair<GameObject, GameObject> entry in tagRows)
        {
            entry.Key.GetComponent<WorkerTagMovement>().frequency = int.Parse(entry.Value.GetComponentsInChildren<Text>()[4].text);
            entry.Key.name = entry.Value.GetComponentsInChildren<Text>()[1].text;
            Debug.Log("!!!!!!!!!"+entry.Value.GetComponentsInChildren<Text>()[4].text);
        }
    }
}
