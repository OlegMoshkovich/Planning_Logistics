using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(DBManager))]
public class AssetManager : MonoBehaviour {
	//Make Unique Manager
	public static AssetManager main;
    //Database link
    public DBManager db;


    //Create Folders in Tree
    GameObject workers;
    GameObject indicatorsParent;
    GameObject assets;
    GameObject forklifts;
    GameObject ladders;
    GameObject otherAssets;

    public void Awake(){
		main = this;

        //Set up Tree
        workers = new GameObject();
        workers.name = "Workers";
        assets = new GameObject();
        assets.name = "Assets";
        forklifts = new GameObject();
        forklifts.name = "Forklifts";
        forklifts.transform.parent = assets.transform;
        ladders = new GameObject();
        ladders.name = "Ladders";
        ladders.transform.parent = assets.transform;
        otherAssets = new GameObject();
        otherAssets.name = "Others";
        otherAssets.transform.parent = assets.transform;
        
    }

    private void Start()
    {
        db = GetComponent<DBManager>();

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


	public void createNewAsset(string setAssetType = "other", Texture2D cursor = null)
	{
		drawerActive = true;
		assetType = setAssetType;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        Debug.Log ("the drawer is active");
	}

    public void createAssetFromJSON(SimpleJSON.JSONNode doc)
    {
        try {
            Debug.Log("Create From JSON: " + doc.ToString());
            string type = doc["type"].Value;
            Vector3 position = new Vector3(float.Parse(doc["position"]["x"].Value), float.Parse(doc["position"]["y"].Value), float.Parse(doc["position"]["z"].Value));
            GameObject go = (GameObject)Instantiate(Resources.Load<GameObject>("AssetsLibrary/" + type), position, transform.rotation);
            if (go.GetComponent<SyncedAsset>() == null) Debug.LogError("Prefab missing Synched Asset");
            else
            {
                go.GetComponent<SyncedAsset>().UpdateWithJSON(doc.ToString());
            }
            AddToTree(go, type);
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

	private void placeAsset(string assetType){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetKeyDown ("p") && assetType != null) {
			Debug.Log ("p is pressed");
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				//Debug.Log ("something is hit");	
				GameObject go = (GameObject) Instantiate(Resources.Load<GameObject>("AssetsLibrary/" + assetType), new Vector3(hit.point.x,hit.point.y,hit.point.z), Quaternion.Euler (0, 0, 0));
                if (go.GetComponent<SyncedAsset>() != null) go.GetComponent<SyncedAsset>().type = assetType;
                go.name = assetType;

                //Return cursor to default
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

                AddToTree(go, assetType);

			}
		}
	}

    private void AddToTree(GameObject go, string assetType)
    {
        switch (assetType)
        {
            case "Worker":
                go.transform.parent = workers.transform;
                TreeViewManager.main.TreeView.AddChild(workers, go);
                break;
            case "Ladder":
                go.transform.parent = ladders.transform;
                TreeViewManager.main.TreeView.AddChild(ladders, go);
                break;
            case "Forklift":
                go.transform.parent = forklifts.transform;
                TreeViewManager.main.TreeView.AddChild(forklifts, go);
                break;
            case "Hazard":
                go.transform.parent = HazardManager.main.Hazards.transform;
                TreeViewManager.main.TreeView.AddChild(HazardManager.main.Hazards, go);
                break;
            default:
                go.transform.parent = otherAssets.transform;
                TreeViewManager.main.TreeView.AddChild(otherAssets, go);
                break;
        }
        TreeViewManager.main.updateTreeText();
    }
    public void DeleteAsset(GameObject asset)
    {
        if(asset != null){
            SyncedAsset sa = asset.GetComponent<SyncedAsset>();
            if (sa != null)
            {
                db.deleteAsset(sa._id, sa._rev);
            }
            Destroy(asset);
        }
        else
        {
            Debug.Log("Asset was null");
        }
    }

	void Update () {
		if (drawerActive) {
			placeAsset (assetType);
		}   
    }
    //create asset from CludantDB doc
    public void CreateAssetFromDoc(string doc)
    {

    }
    
}
