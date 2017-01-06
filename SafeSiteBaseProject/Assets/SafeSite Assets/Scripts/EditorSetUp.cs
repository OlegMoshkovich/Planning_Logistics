using UnityEngine;
using System.Collections;

public class EditorSetUp : MonoBehaviour {
    public GameObject Model;
    public static EditorSetUp main;
    // Use this for initialization

    private void Awake()
    {
        main = this;
    }
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
