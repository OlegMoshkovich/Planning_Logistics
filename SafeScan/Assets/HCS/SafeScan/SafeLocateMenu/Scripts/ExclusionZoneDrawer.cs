using UnityEngine;
using System.Collections;
using System.Collections.Generic;


    public class ExclusionZoneDrawer : MonoBehaviour
    {
        private GameObject lineRendererGO;
        private LineRenderer lineRenderer;
        private int lineCount = 0;
        public bool drawerActive = true;
        public List<Vector3> linePoints = new List<Vector3>();

        private float yValue;

        public void setDrawerActive(bool value)
        {
            drawerActive = value;
        }
        // Use this for initialization
        void Start()
        {
            lineRendererGO = new GameObject();
            lineRendererGO.AddComponent<LineRenderer>();
            lineRenderer = lineRendererGO.GetComponent<LineRenderer>();
        lineRenderer.material = ExclusionZoneManager.main.exclusionZoneWarningMaterial;
            linePoints = new List<Vector3>();
        }

        // Update is called once per frame
        void Update()
        {
            if (drawerActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mouseClick = Input.mousePosition;
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100.0f))
                    {
                        if (yValue == null) yValue = hit.point.y+0.2f;
                        Vector3 worldPoint = new Vector3(hit.point.x, yValue, hit.point.z);
                        linePoints.Add(worldPoint);
                        GameObject newPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        newPoint.GetComponent<Renderer>().material = ExclusionZoneManager.main.exclusionZoneDangerMaterial;
                        newPoint.transform.position = worldPoint;
                        UpdateLine();
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    drawerActive = false;
                    ClearDrawing();
                }
            }
        }
        void UpdateLine()
        {
            lineRenderer.SetVertexCount(linePoints.Count);

            for (int i = lineCount; i < linePoints.Count; i++)
            {
                lineRenderer.SetPosition(i, linePoints[i]);
            }
            lineCount = linePoints.Count;
            if (lineCount > 2)
            {
                if (Vector3.Magnitude(linePoints[0] - linePoints[lineCount - 1]) < 1)
                {
                    drawerActive = false;
                    CreateMesh();
                    
            }

            }
        }
        void CreateMesh()
        {

            // Use the triangulator to get indices for creating triangles
            Vector2[] vertices2D = new Vector2[lineCount];
            for (int i = 0; i < lineCount; i++)
            {
                vertices2D[i].x = linePoints[i].x;
                vertices2D[i].y = linePoints[i].z;
            }

            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();
            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }

            // Create the mesh
            Mesh msh = new Mesh();
            msh.vertices = linePoints.ToArray();
            //msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();

            // Set up game object with mesh;
            GameObject newExclusionZone = new GameObject();
            newExclusionZone.transform.parent = ExclusionZoneManager.main.exclusionZones.transform;
            newExclusionZone.AddComponent(typeof(MeshRenderer));
            newExclusionZone.name = "Exclusion Zone";
            newExclusionZone.transform.Translate(new Vector3(0, 0.1f, 0));
            MeshFilter filter = newExclusionZone.AddComponent(typeof(MeshFilter)) as MeshFilter;
            filter.mesh = msh;
            newExclusionZone.GetComponent<MeshRenderer>().material = ExclusionZoneManager.main.exclusionZoneDangerMaterial;
            newExclusionZone.AddComponent<MeshCollider>();
        //Add to list of Exclusion Zones
            ExclusionZone ez = new ExclusionZone();
            ez.gameObject = newExclusionZone;
            ExclusionZoneManager.main.listOfExclusionZones.Add(ez);
        //Update Tree
            TreeViewManager.main.TreeView.AddChild(ExclusionZoneManager.main.exclusionZones, newExclusionZone);
            //Clear Drawing
            ClearDrawing();

    }
    public void ClearDrawing()
    {
        lineRenderer.SetVertexCount(0);
        linePoints = new List<Vector3>();
    }


}
/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class ExclusionZoneManager : MonoBehaviour
{
    public List<ExclusionZone> ExclusionZones = new List<ExclusionZone>();
    public Material DangerMaterial;
    public Material WarningMaterial;

    IEnumerable<GameObject> ListOfActors = new List<GameObject>();

    private bool exclusionZonesVisible = true;

    public void ToggleZones()
    {
        //TurnZonesOn/OFF
    }
    // Use this for initialization
    void Start()
    {
        if (WorkerManager.main == null) Debug.LogError("missing Worker Manager");
    }

    // Update is called once per frame
    void Update()
    {
        ListOfActors = WorkerManager.main.listOfWorkers;
    }
}
*/
