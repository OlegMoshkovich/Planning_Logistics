using UnityEngine;
using System.Collections;
using Battlehub.UIControls;

public class TreeButtonsManager : MonoBehaviour {
    public TreeView treeview;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void CreateWorker()
    {
        Instantiate(GameObject.Find("John"));
        treeview.SendMessage("Start");
    }
    public void CreateFolder()
    {
        GameObject newFolder = new GameObject();
        newFolder.name = "New Folder";
        newFolder.AddComponent<Folder>();
        treeview.SendMessage("Start");
    }
}
