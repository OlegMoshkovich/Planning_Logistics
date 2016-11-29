using UnityEngine;
using System.Collections;

public class FolderTreeItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnInputChange(string value)
    {
        Debug.Log(value);
        this.name = value;
    }
}
