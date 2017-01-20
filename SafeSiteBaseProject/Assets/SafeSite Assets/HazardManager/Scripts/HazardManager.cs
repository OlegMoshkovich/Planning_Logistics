using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardManager : MonoBehaviour {
    public GameObject HazardMarkerPrefab;
    public List<GameObject> listOfHazards = new List<GameObject>();
    public List<GameObject> listOfCollisions = new List<GameObject>();
    public GameObject Hazards;
    public GameObject Collisions;


    public static HazardManager main;
	// Use this for initialization
	void Start () {
        main = this;
        Hazards = new GameObject();
        Hazards.name = "Hazards";
        TreeViewManager.main.TreeView.Add(Hazards);

        Collisions = new GameObject();
        Collisions.name = "Collisions";
        TreeViewManager.main.TreeView.Add(Collisions);
    }
    public void AddCollision(Collider collider)
    {
        Debug.Log("Collision entered");
        /*
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "Collision: " + collision.collider.name + " - " + collision.gameObject.name;
        go.transform.position = collision.contacts[0].point;
        go.GetComponent<Renderer>().material.color = Color.yellow;
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        go.transform.parent = Collisions.transform;
        TreeViewManager.main.TreeView.AddChild(Collisions, go);
        */
    }


}
