﻿using UnityEngine;
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
            CameraSwitch myCameraSwitch = GameObject.FindObjectOfType<CameraSwitch>();
            myCameraSwitch.SetTarget(selectedGO.transform);
        }
	}
  
}
