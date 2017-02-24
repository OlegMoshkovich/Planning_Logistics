using UnityEngine;

public class HazardManager : MonoBehaviour
{
    public GameObject HazardMarkerPrefab;
    public GameObject Hazards;
    public GameObject Collisions;
    public GameObject Edges;


    public static HazardManager main;
    // Use this for initialization
    void Start()
    {
        main = this;
        Hazards = new GameObject("Hazards");
        TreeViewManager.main.TreeView.Add(Hazards);

        Collisions = new GameObject("Collisions");
        TreeViewManager.main.TreeView.AddChild(AssetManager.main.indicatorsParent,Collisions);
        Edges = new GameObject("Fall Risks");
        TreeViewManager.main.TreeView.AddChild(AssetManager.main.indicatorsParent, Edges);


    }
    public void AddCollision(Collider collider)
    {
        //Debug.Log("Collision entered");
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
    public void AddEdge(Vector3 position)
    {
        GameObject go = Instantiate(HazardMarkerPrefab, position, Camera.main.transform.rotation) as GameObject;
        go.name = "Fall Indicator";
        go.transform.parent = Edges.transform;
        TreeViewManager.main.TreeView.AddChild(Edges, go);
    }


}

