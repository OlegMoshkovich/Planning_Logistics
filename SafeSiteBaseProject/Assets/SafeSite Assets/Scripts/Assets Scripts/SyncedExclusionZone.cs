using UnityEngine;
using System.Collections;

    public class SyncedExclusionZone : SyncedAsset
    {
        public Vector3[] points;
        public bool dynamicSize = false;
        public bool alert = true;
        public string shape; // "Circle", "rectangle", "Mesh "
        public delegate void ExclusionZoneAction (Collision collision);
        public static event ExclusionZoneAction OnExclusionZoneEnter;
        public static event ExclusionZoneAction OnExclusionZoneExit;

        public void ExportExclusionZone()
        {
        Debug.Log(JsonUtility.ToJson(this.gameObject.AddComponent<SyncedHazard>()));
        }

    protected void Start()
    {
        if (TreeViewManager.main.TreeView != null) TreeViewManager.main.TreeView.AddChild(ExclusionZoneManager.main.exclusionZones, gameObject);
    }

    public void updateMesh() //Run when points of EZ change
    {
        gameObject.transform.parent = ExclusionZoneManager.main.exclusionZones.transform;
        gameObject.transform.position = points[0];
        //Refresh Cube points
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Vector3 point in points)
        {
            GameObject exclusionZonePoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            exclusionZonePoint.name = "Point";
            exclusionZonePoint.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            exclusionZonePoint.transform.parent = gameObject.transform;
            exclusionZonePoint.transform.position = point;
            exclusionZonePoint.GetComponent<Renderer>().material = ExclusionZoneManager.main.exclusionZoneDangerMaterial;
        }
        // Use the triangulator to get indices for creating triangles
        Vector2[] vertices2D = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            vertices2D[i].x = points[i].x - points[0].x;
            vertices2D[i].y = points[i].z - points[0].y;
        }

        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < points.Length; i++)
        {
            vertices[i] = points[i] - points[0];
        }
        // Create the mesh
        Mesh msh = new Mesh()
        {
            vertices = vertices,
            triangles = indices
        };
        msh.RecalculateNormals();
        msh.RecalculateBounds();
        // Set up game object with mesh;
       
        MeshRenderer newExclusionZone_meshRenderer = gameObject.AddComponent<MeshRenderer>();
        gameObject.transform.Translate(new Vector3(0, 0.1f, 0));
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = msh;
        newExclusionZone_meshRenderer.material = ExclusionZoneManager.main.exclusionZoneDangerMaterial;
        gameObject.AddComponent<MeshCollider>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (alert)
        {
            //Trigger Event if it has subscriberssy
            //if (OnExclusionZoneEnter != null) OnExclusionZoneEnter(collider);
            //Call ExclusionZoneManagerHandler
            ExclusionZoneManager.main.OnExclusionZoneEnter_Handler(collider);
        }
        
    }
    public void OnTriggerExit(Collider collider)
    {
        if (alert)
        {
            //Trigger Event if it has subscribers
            //if (OnExclusionZoneEnter != null) OnExclusionZoneEnter(collider);
            //Call ExclusionZoneManagerHandler
            ExclusionZoneManager.main.OnExclusionZoneExit_Handler(collider);
        }

    }
    public void OnCollisionStay(Collision collision)
    {
        ExclusionZoneManager.main.OnExclusionZoneStay_Handler(collision);
    }
    public void OnCollisionExit(Collision collision)
    {
        //Trigger Event if it has subscribers
        //if (OnExclusionZoneEnter != null) OnExclusionZoneExit(collision);
        //Call ExclusionZoneManagerHandler
        //ExclusionZoneManager.main.OnExclusionZoneExit_Handler(collision);
    }
    
}


