using UnityEngine;
using System.Collections;
using Battlehub.UIControls;
using UnityEngine.EventSystems;

public class TreeButtonsManager : MonoBehaviour
{
    public TreeView treeview;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	   
	}

    public void CreateFolder()
    {
        GameObject newFolder = new GameObject();
        newFolder.name = "New Folder";
        newFolder.AddComponent<Folder>();
        TreeViewManager.main.TreeView.Add(newFolder);
    }
}
