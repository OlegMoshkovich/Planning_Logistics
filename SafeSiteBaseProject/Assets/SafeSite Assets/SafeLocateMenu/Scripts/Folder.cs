using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Folder : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnEndEdit()
    {
        this.GetComponentInChildren<Text>().text = this.GetComponentInChildren<InputField>().text;
        Destroy(this.GetComponentInChildren<InputField>());
    }
}
