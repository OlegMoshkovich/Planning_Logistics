using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.Networking;
using System;

public class DBManager : MonoBehaviour {
    public string username = "7913dc07-9b69-4a17-8329-cb43b00bbd14-bluemix";
    public string password = "5d53ba229591931afc62e00011b05687a17ce40012615c32a826b8219f106d2d";
    public string databaseURL = "https://7913dc07-9b69-4a17-8329-cb43b00bbd14-bluemix.cloudant.com/";
    public Dictionary<string, string> projectName = new Dictionary<string, string>(){{"61Broadway", "broadway61/" },
                                                                                { "Queens Plaza", "queens_plaza/"},
                                                                                { "DGH Lincoln Center", "lincoln_center/"},
                                                                                { "VR_Forklift_06", "forkliftvr/"}};

    private WebRequest webRequest;

    private string encodedAuthorization;

    void Start () {
        webRequest = gameObject.AddComponent<WebRequest>();
        encodedAuthorization = "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(username + ":" + password));
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
        var unityWebRequest = UnityWebRequest.Get(url);
        unityWebRequest.SetRequestHeader("Authorization", encodedAuthorization);
        webRequest.SendUnityWebRequest(unityWebRequest, getAllAssetsHandler);
#endif
        DestroyAllSynchedAssets();
    }

    public void getAllAssetsHandler(UnityWebRequest req)
    {
        Debug.Log(req.downloadHandler.text);
        var n = JSON.Parse(req.downloadHandler.text);
        for (int i = 0; i < n["rows"].Count; i++)
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
        UploadHandlerRaw uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(postData));
        DownloadHandler downloadHandler = new DownloadHandlerBuffer();
        var unityReq = new UnityWebRequest()
        {
            url = url,
            uploadHandler = uploadHandler,
            downloadHandler = downloadHandler,
            method = UnityWebRequest.kHttpVerbPOST
        };
        unityReq.SetRequestHeader("Content-Type", "application/json");
        unityReq.SetRequestHeader("Authorization", encodedAuthorization);
        webRequest.SendUnityWebRequest(unityReq, updateAssetsHandler);
    }

    private void updateAssetsHandler(UnityWebRequest req)
    {
        if(req.downloadHandler!=null) if(req.downloadHandler.text !=null) Debug.Log("updateAssetsHandler : " + req.downloadHandler.text);
        else
            {
                var n = JSON.Parse(req.downloadHandler.text);
                for (int i = 0; i < n["rows"].Count; i++)
                {
                    Debug.Log(n["rows"][i]["doc"].ToString());
                    saArray[i]._id = n[i]["id"];
                    saArray[i]._rev = n[i]["rev"];
                }
            }
        
    }
    public void deleteAsset(string _id, string _rev)
    {
        string url = databaseURL + projectName[SceneManager.GetActiveScene().name] + "_bulk_docs";
        string postData = "{\"docs\":[{\"_id\":\"" + _id + "\",\"_rev\":\"" + _rev + "\",\"_deleted\": true}]}";
        Debug.Log("Post Data: "+postData);
        UploadHandlerRaw uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(postData));
        var unityReq = new UnityWebRequest()
        {
            url = url,
            uploadHandler = uploadHandler,
            method = UnityWebRequest.kHttpVerbPOST
        };
        unityReq.SetRequestHeader("Content-Type", "application/json");
        unityReq.SetRequestHeader("Authorization", encodedAuthorization);
        webRequest.SendUnityWebRequest(unityReq, deleteAssetHandler);
    }
    public void deleteAssetHandler(UnityWebRequest req)
    {
        //if(req.downloadHandler.text != null) Debug.Log(req.downloadHandler.text);
    }

    

    
}
