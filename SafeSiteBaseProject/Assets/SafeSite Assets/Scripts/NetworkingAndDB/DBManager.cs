using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using SimpleJSON;

public class DBManager : MonoBehaviour {
    public string databaseURL = "https://7913dc07-9b69-4a17-8329-cb43b00bbd14-bluemix:5d53ba229591931afc62e00011b05687a17ce40012615c32a826b8219f106d2d@7913dc07-9b69-4a17-8329-cb43b00bbd14-bluemix.cloudant.com/";
    public Dictionary<string, string> projectName = new Dictionary<string, string>(){{"61Broadway", "broadway61/" },
                                                                                { "Queens Plaza", "queens_plaza/"},
                                                                                { "DGH Lincoln Center", "lincoln_center/"},
                                                                                { "VR_Forklift_06", "forkliftvr/"}};

    [HideInInspector]
    public WebRequest req;

    void Start () {
        req = this.gameObject.AddComponent<WebRequest>();
    }

    public void GetAllAssetsTrigger()
    {

#if UNITY_WEBGL
        string url = databaseURL + projectName[SceneManager.GetActiveScene().name] + "_all_docs?include_docs=true";
        var headers = new Dictionary<string, string>();
        headers.Add("X-HTTP-Method-Override", "GET");
        //headers.Add("Authorization", "Basic NzkxM2RjMDctOWI2OS00YTE3LTgzMjktY2I0M2IwMGJiZDE0LWJsdWVtaXg6NWQ1M2JhMjI5NTkxOTMxYWZjNjJlMDAwMTFiMDU2ODdhMTdjZTQwMDEyNjE1YzMyYTgyNmI4MjE5ZjEwNmQyZA==");
        headers.Add("Content-Type", "application/json");
        headers.Add("Access-Control-Allow-Credentials", "true");
        headers.Add("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
        headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        headers.Add("Access-Control-Allow-Origin", "*");
        req.HTTPGETRequest(url, getAllAssetsHandler, headers);
#else
        string url = databaseURL + projectName[SceneManager.GetActiveScene().name] + "_all_docs?include_docs=true";
        req.HTTPGETRequest(url, getAllAssetsHandler);
#endif
        DestroyAllSynchedAssets();
    }

    public void getAllAssetsHandler()
    {
        Debug.Log(req.download.text);
        var n = JSON.Parse(req.download.text);    
        for(int i =0; i<n["rows"].Count; i++)
        {
            AssetManager.main.createAssetFromJSON(n["rows"][i]["doc"]);
        }
        if (CameraManager.main != null) CameraManager.main.SetUpCameras();
        else Debug.Log("Missing Camera Switch"); 
    }

    private SyncedAsset[] saArray;
    private SyncedHazard[] shArray;

    public void DestroyAllSynchedAssets()
    {
        saArray = Component.FindObjectsOfType<SyncedAsset>();
        Debug.Log("Destroy all assets:" + saArray.Length.ToString());
        foreach (SyncedAsset sa in saArray)
        {
            Debug.Log(sa.gameObject.name);
            Destroy(sa.gameObject);
        }
    }

    public void updateAssetsTrigger()
    {
        string url = databaseURL + projectName[SceneManager.GetActiveScene().name] + "_bulk_docs";
        var headers = new Dictionary<string, string>();
        headers.Add("X-HTTP-Method-Override", "POST");
        headers.Add("Content-Type", "application/json");
        string postData = "{\"docs\":[";

        saArray = FindObjectsOfType<SyncedAsset>();
        shArray = HazardManager.main.Hazards.GetComponentsInChildren<SyncedHazard>();
        for(int i=0; i < saArray.Length; i++)
        {
            if (saArray[i].sa_type != "Hazard")
            {
                JSONNode n = JSON.Parse(JsonUtility.ToJson(saArray[i]));
                if (n["_id"] == null || n["_rev"] == null)
                {
                    n.Remove("_id");
                    n.Remove("_rev");
                }
                postData += n.ToString();
                if (i != saArray.Length - 1) postData += ",";
            }
        }
        if (shArray.Length > 0 && postData[postData.Length-1] != ',' && postData[postData.Length - 1] != '[') postData += ",";
        for (int i = 0; i < shArray.Length; i++)
        {
                JSONNode n = JSON.Parse(JsonUtility.ToJson(shArray[i]));
                if (n["_id"] == null || n["_rev"] == null)
                {
                    n.Remove("_id");
                    n.Remove("_rev");
                }
                postData += n.ToString();
                if (i != shArray.Length - 1) postData += ",";
        }
        postData += "]}";
        Debug.Log("Posting Data: " + postData);
        req.HTTPRequest(url, updateAssetsHandler, System.Text.Encoding.UTF8.GetBytes(postData), headers);
    }

    private void updateAssetsHandler()
    {
#if UNITY_EDITOR
        Debug.Log("updateAssetsHandler : " + req.download.text);
#endif
        var n = JSON.Parse(req.download.text);
        for (int i = 0; i < n["rows"].Count; i++)
        {
            Debug.Log(n["rows"][i]["doc"].ToString());
            saArray[i]._id = n[i]["id"];
            saArray[i]._rev = n[i]["rev"];
        }
    }
    public void deleteAsset(string _id, string _rev)
    {
        string url = databaseURL + projectName[SceneManager.GetActiveScene().name] + "_bulk_docs";
        string postData = "{\"docs\":[{\"_id\":\""+_id+ "\",\"_rev\":\"" + _rev + "\",\"_deleted\": true}]}";
        var headers = new Dictionary<string, string>();
        headers.Add("X-HTTP-Method-Override", "POST");
        headers.Add("Content-Type", "application/json");
        Debug.Log(postData);
        req.HTTPRequest(url, deleteAssetHandler, System.Text.Encoding.UTF8.GetBytes(postData), headers);
    }
    public void deleteAssetHandler()
    {
#if UNITY_EDITOR
        Debug.Log(req.download.text);
#endif
    }

    

    
}
