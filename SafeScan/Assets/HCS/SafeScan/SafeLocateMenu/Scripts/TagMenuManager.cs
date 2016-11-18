using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class TagMenuManager : MonoBehaviour {

    // Use this for initialization
    private List<Tag> listOfTags = new List<Tag>();
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
            listOfTags = GameObject.FindObjectOfType<mqttManager>().listOfTags;
            for(int i=0; i<listOfTags.Count; i++)
            {
                GameObject newRow = (GameObject) UnityEngine.Object.Instantiate(dataRow, GameObject.Find("TagTable").transform);
                newRow.transform.position = new Vector3(dataRow.transform.position.x, dataRow.transform.position.y - 40 * (i+1), dataRow.transform.position.z);
                newRow.transform.localScale = Vector3.one;
                newRow.GetComponentsInChildren<Text>()[3].text = listOfTags[i].Frequency.ToString();
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
}
