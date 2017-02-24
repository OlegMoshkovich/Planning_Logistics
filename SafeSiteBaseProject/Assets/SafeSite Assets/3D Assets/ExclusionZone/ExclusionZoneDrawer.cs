using UnityEngine;

using System.Collections.Generic;


    public class ExclusionZoneDrawer : MonoBehaviour
    {
        private GameObject lineRendererGO;
        private LineRenderer lineRenderer;
        private int lineCount = 0;
        public bool drawerActive = true;
        private List<GameObject> cubePoints = new List<GameObject>();
    public float cubeScale = 0.3f;

        public void setDrawerActive(bool value)
        {
            drawerActive = value;
        }

        void Start()
        {
            lineRendererGO = new GameObject("Exclusion Zone Drawer Line");
            lineRenderer  = lineRendererGO.AddComponent<LineRenderer>();
            lineRenderer.material = ExclusionZoneManager.main.exclusionZoneWarningMaterial;
            cubePoints = new List<GameObject>();
        }

        void Update()
        {
            if (drawerActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100.0f))
                    {
                        Vector3 worldPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        //worldPoint -= ray.direction * 0.1f;
                        GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cubePoints.Add(newCube);
                        newCube.transform.localScale = new Vector3(cubeScale, cubeScale, cubeScale);
                        newCube.GetComponent<Renderer>().material = ExclusionZoneManager.main.exclusionZoneDangerMaterial;
                        newCube.transform.position = worldPoint;
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
            lineRenderer.SetVertexCount(cubePoints.Count);
            for (int i = 0; i < cubePoints.Count; i++)
            {
                lineRenderer.SetPosition(i, cubePoints[i].transform.position);
            }
            lineCount = cubePoints.Count;
            if (lineCount > 2)
            {
                if (Vector3.Distance(cubePoints[0].transform.position, cubePoints[cubePoints.Count - 1].transform.position) < 1)
                {
                    drawerActive = false;
                    CreateMesh();
                }
            }
        }
  void CreateMesh(){
        Vector3[] vertices = new Vector3[cubePoints.Count];
        for (int i = 0; i < cubePoints.Count; i++){
            vertices[i] = cubePoints[i].transform.position;
        }
        var go = (GameObject)Instantiate(Resources.Load<GameObject>("AssetsLibrary/" + AssetType.ExclusionZone));
        go.name = "Exclusion Zone";
        var sez = go.GetComponent<SyncedExclusionZone>();
        sez.points = vertices;
        sez.updateMesh();
        ClearDrawing();

    }
    public void ClearDrawing()
    {
        lineRenderer.SetVertexCount(0);
        foreach (GameObject go in cubePoints)
        {
            Destroy(go);
        }
        cubePoints.Clear();
    }


}

