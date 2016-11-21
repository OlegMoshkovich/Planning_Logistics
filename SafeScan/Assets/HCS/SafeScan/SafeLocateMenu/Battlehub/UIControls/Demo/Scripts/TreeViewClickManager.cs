using UnityEngine;
using System.Collections;
using Battlehub.UIControls;

public class TreeViewClickManager : MonoBehaviour {
    public TreeView Treeview;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0) && Treeview.SelectedItem !=null)
        {
            GameObject selectedGO = (GameObject)Treeview.SelectedItem;
            Camera.main.transform.position = new Vector3(selectedGO.transform.position.x, selectedGO.transform.position.y + 2, selectedGO.transform.position.z - 5);
        }
	}
  
}
