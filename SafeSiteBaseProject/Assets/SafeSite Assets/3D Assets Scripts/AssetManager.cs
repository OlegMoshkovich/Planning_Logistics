using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AssetManager : MonoBehaviour {
	//Make Unique Manager
	public static AssetManager main;

	//Lists of Assets
	public List<GameObject> listOfWorkers = new List<GameObject>();
	public List<GameObject> listOfLadders = new List<GameObject> ();
	public List<GameObject> listOfSafetyNets = new List<GameObject>();
	public List<GameObject> listOfForklifts = new List<GameObject> ();
	public List<GameObject> listOfOthers = new List<GameObject> ();

    //Create Folders in TRee
    GameObject workers;
    GameObject indicatorsParent;
    GameObject assets;
    GameObject forklifts;
    GameObject ladders;
    GameObject otherAssets;

    public void Awake(){
		main = this;

        //Set up Tree
        indicatorsParent = new GameObject();
        indicatorsParent.name = "Safescan indicators 6";
        workers = new GameObject();
        workers.name = "Workers 32/100";

        assets = new GameObject();
        assets.name = "Assets 24";
        forklifts = new GameObject();
        forklifts.name = "Forklits " + forklifts.transform.childCount;
        forklifts.transform.parent = assets.transform;
        ladders = new GameObject();
        ladders.name = "Ladders " + forklifts.transform.childCount;
        ladders.transform.parent = assets.transform;
        otherAssets = new GameObject();
        otherAssets.name = "Others " + otherAssets.transform.childCount;
        otherAssets.transform.parent = assets.transform;
        
    }

    private void Start()
    {
        //Set up Tree
        TreeViewManager.main.TreeView.Add(workers);
        TreeViewManager.main.TreeView.Add(indicatorsParent);
        TreeViewManager.main.TreeView.Add(assets);
        TreeViewManager.main.TreeView.AddChild(assets, forklifts);
        TreeViewManager.main.TreeView.AddChild(assets, ladders);
        TreeViewManager.main.TreeView.AddChild(assets, otherAssets);
    }

	//Code to Draw Assets
	private GameObject assetToDraw;
	private string assetType;
	private bool drawerActive = true;


	public void createNewAsset( GameObject assetPrefab, string setAssetType = "other", Texture2D cursor = null)
	{
		drawerActive = true;
		assetToDraw = assetPrefab;
		assetType = setAssetType;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        Debug.Log ("the drawer is active");
	}

	private void placeAsset(GameObject assetPrefab){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetKeyDown ("p") & assetPrefab != null) {
			Debug.Log ("p is pressed");
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				//Debug.Log ("something is hit");	
				GameObject go = (GameObject) Instantiate(assetPrefab,new Vector3(hit.point.x,hit.point.y,hit.point.z), Quaternion.Euler (-90, 0, 0));
                go.name = assetPrefab.name;
                //Add to TreeView
                
                Debug.Log ("x:" + hit.point.x);
				Debug.Log ("y:" +hit.point.y);
				Debug.Log ("z:" +hit.point.z);

                //Return cursor to default
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

                //Add new GameObject to List
                switch (assetType) {
				case "worker":
					listOfWorkers.Add (go);
                    go.transform.parent = workers.transform;
                    TreeViewManager.main.TreeView.AddChild(workers, go);
                    //workers.name = "Workers (" + workers.transform.childCount + ")";
                        break;
				case "safetyNet":
					listOfSafetyNets.Add (go);
                    go.transform.parent = otherAssets.transform;
                    TreeViewManager.main.TreeView.AddChild(otherAssets, go);
					break;
				case "ladder":
					listOfLadders.Add (go);
                        go.transform.parent = ladders.transform;
                        TreeViewManager.main.TreeView.AddChild(ladders, go);
                        break;
				case "forklift":
					listOfForklifts.Add (go);
                    go.transform.parent = forklifts.transform;
                    TreeViewManager.main.TreeView.AddChild(forklifts, go);
                        break;
				case "other":
					listOfOthers.Add (go);
                    go.transform.parent = otherAssets.transform;
                    TreeViewManager.main.TreeView.AddChild(otherAssets, go);
                        break;
				}

			}
		}
	}

	void Update () {
		if (drawerActive) {
			placeAsset (assetToDraw);
		}   
    }
    
}
