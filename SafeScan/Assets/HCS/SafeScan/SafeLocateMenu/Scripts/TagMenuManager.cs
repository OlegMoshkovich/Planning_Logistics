using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class TagMenuManager : MonoBehaviour {

    public Dictionary<GameObject, GameObject> tagRows  = new Dictionary<GameObject, GameObject>(); //<Worker, Tag Row>
    public GameObject dataRow;
	void Start () {
        RefreshTagTable();
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
                //Create one Row for every worker.
                GameObject selectedRow = (GameObject)UnityEngine.Object.Instantiate(dataRow, this.transform);
                //Bind Worker to Tag
                tagRows.Add(WorkerManager.main.listOfWorkers[i], selectedRow);
                //Move Row to end of list and show text
                selectedRow.transform.localScale = Vector3.one;
                selectedRow.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -10 + 30 * (i + 1), 35);
                selectedRow.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 420, 665);
                selectedRow.GetComponent<TagRow>().NameText.text = WorkerManager.main.listOfWorkers[i].name;
                selectedRow.GetComponent<TagRow>().QTrackKHzText.text = WorkerManager.main.listOfWorkers[i].GetComponent<WorkerTagMovement>().frequency.ToString();
                selectedRow.GetComponent<TagRow>().HCSTagIDText.text = WorkerManager.main.listOfWorkers[i].GetComponent<WorkerTelemetry>().macAddress;

            }
        }
        catch (Exception error)
        {
            Debug.LogError(error.ToString());
        }
    }

    public void SubmitData()
    {
        foreach (KeyValuePair<GameObject, GameObject> entry in tagRows)
        {
            entry.Key.GetComponent<WorkerTagMovement>().frequency = int.Parse(entry.Value.GetComponent<TagRow>().QTrackKHzText.text);
            entry.Key.name = entry.Value.GetComponent<TagRow>().NameText.text;
            entry.Key.GetComponent<WorkerTelemetry>().macAddress = entry.Value.GetComponent<TagRow>().HCSTagIDText.text;
        }
    }
}
