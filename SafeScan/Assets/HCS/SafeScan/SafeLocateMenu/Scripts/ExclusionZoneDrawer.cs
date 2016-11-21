using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Safescan
{
    public class ExclusionZoneDrawer : MonoBehaviour
    {
        private GameObject newExclusionZone;
        private LineRenderer lineRenderer;
        private int lineCount = 0;
        public bool drawerActive = true;
        public List<Vector3> linePoints = new List<Vector3>();
        public Material exclusionZoneMaterial;

        private float yValue;

        public void setDrawerActive(bool value)
        {
            drawerActive = value;
        }
        // Use this for initialization
        void Start()
        {
            newExclusionZone = new GameObject();
            newExclusionZone.AddComponent<LineRenderer>();
            lineRenderer = newExclusionZone.GetComponent<LineRenderer>();
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
                        newPoint.GetComponent<Renderer>().material = exclusionZoneMaterial;
                        newPoint.transform.position = worldPoint;
                        UpdateLine();
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    drawerActive = false;
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
                    Destroy(lineRenderer);
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
            newExclusionZone.AddComponent(typeof(MeshRenderer));
            newExclusionZone.name = "New Exclusion Zone";
            newExclusionZone.transform.Translate(new Vector3(0, 0.1f, 0));
            MeshFilter filter = newExclusionZone.AddComponent(typeof(MeshFilter)) as MeshFilter;
            filter.mesh = msh;
            newExclusionZone.GetComponent<MeshRenderer>().material = exclusionZoneMaterial;
            newExclusionZone.AddComponent<MeshCollider>();
        }


    }
}
