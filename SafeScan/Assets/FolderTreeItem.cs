using UnityEngine;
using System.Collections;
using Battlehub.UIControls;

public class FolderTreeItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnInputChange(string value)
    {
        if(this.GetComponent<ItemContainer>()!= null)
        {
            ItemContainer itemContainer = this.GetComponent<ItemContainer>();
            GameObject item = (GameObject)itemContainer.Item;
            item.name = value;
        }
            
    }
}
