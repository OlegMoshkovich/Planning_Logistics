using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


[RequireComponent(typeof(DBManager))]
public class AssetManager : MonoBehaviour {
	//Make Unique Manager
	public static AssetManager main;
    //Database link
    [HideInInspector]
    public DBManager db;


    //Create Folders in Tree
    GameObject workers;
    [HideInInspector]
    public GameObject indicatorsParent;
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
        indicatorsParent = new GameObject();
        indicatorsParent.name = "Safescan Indicators";
        
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
	private AssetType assetType;
	private bool drawerActive = true;


	public void createNewAsset(AssetType setAssetType, Texture2D cursor = null)
	{
		drawerActive = true;
		assetType = setAssetType;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        Debug.Log ("The Asset drawer is active");
	}

    public void createAssetFromJSON(SimpleJSON.JSONNode doc)
    {
        Debug.Log("Create From JSON: " + doc.ToString());
        try {
            string type = doc[S.SA_TYPE].Value;
            Vector3 position = new Vector3(float.Parse(doc[S.SA_POSITION]["x"].Value), float.Parse(doc[S.SA_POSITION]["y"].Value), float.Parse(doc[S.SA_POSITION]["z"].Value));
            GameObject go = (GameObject)Instantiate(Resources.Load<GameObject>("AssetsLibrary/" + type), position, transform.rotation);
            if (go.GetComponent<SyncedAsset>() == null) Debug.LogError("Prefab missing Synched Asset");
            else
            {
                go.GetComponent<SyncedAsset>().UpdateWithJSON(doc.ToString());
            }
            if (!Enum.IsDefined(typeof(AssetType), type))
            {
                Debug.Log("Type from JSON is not an AssetType");
                AddToTree(go, (AssetType) Enum.Parse(typeof(AssetType), "Other"));
            }
            else
            {
                AddToTree(go, (AssetType) Enum.Parse(typeof(AssetType), type));
            }

        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

	private void placeAsset(AssetType assetType){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //If Keyboard place P, if touch screen press screen
		if ((Input.GetKeyDown ("p")) || (((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) ) && !EventSystem.current.IsPointerOverGameObject())) {
            if (Input.touchCount == 1) drawerActive = false;
			Debug.Log ("p is pressed");
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
                Debug.Log ("Raycast hit, place Asset: " + assetType.ToString());	
                GameObject resource = Resources.Load<GameObject>("AssetsLibrary/" + assetType.ToString());
                if (resource == null) Debug.Log("Missing Resource of type: " + assetType.ToString());
                else
                {
                    GameObject go = (GameObject)Instantiate(resource, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.Euler(0, 0, 0));
                    //Update SynchedAsset
                    SyncedAsset sa = go.GetComponent<SyncedAsset>();
                    if (sa == null) Debug.Log("Resource Missing SyncedAsset");
                    else
                    {
                        sa.sa_type = assetType.ToString();
                        sa.sa_createdBy = UserSettings.main.userName;
                    }

                    go.name = assetType.ToString();

                    //Return cursor to default
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

                    AddToTree(go, assetType);
                }
			}
		}
	}
    private void AddToTree(GameObject go, GameObject parent)
    {
        go.transform.parent = parent.transform;
        TreeViewManager.main.TreeView.AddChild(workers, parent);
    }
    private void AddToTree(GameObject go, AssetType assetType)
    {
        switch (assetType)
        {
            case AssetType.Worker:
                AddToTree(go, workers);
                break;
            case AssetType.Ladder:
                AddToTree(go, ladders);
                break;
            case AssetType.Forklift:
                AddToTree(go, forklifts);
                break;
            case AssetType.Hazard:
                AddToTree(go, HazardManager.main.Hazards);
                break;
            default:
                AddToTree(go, otherAssets);
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
