using UnityEngine;

public class HazardMarkerDrawer : MonoBehaviour {

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseClick = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Vector3 worldPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                worldPoint -= ray.direction*0.1f;
                GameObject newHazardMarker = (GameObject)Instantiate(Resources.Load<GameObject>("AssetsLibrary/Hazard"), HazardManager.main.Hazards.transform);
                newHazardMarker.name = "Hazard";
                SyncedHazard sh = newHazardMarker.GetComponent<SyncedHazard>();
                if (sh == null) Debug.Log("Resource Missing SynchedHazard");
                else
                {
                    sh.sa_type = "Hazard";
                    sh.sa_createdBy = UserSettings.main.userName;
                }
                newHazardMarker.transform.position = worldPoint;
                newHazardMarker.transform.rotation.SetEulerAngles(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
                
				//Move Camera to hazard
				//CameraManager.main.SetTarget(newHazardMarker.transform);

                TreeViewManager.main.TreeView.AddChild(HazardManager.main.Hazards, newHazardMarker);
                TreeViewManager.main.updateTreeText();
                this.enabled = false;
            }
        }
    }
}
