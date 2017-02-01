﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class DBManager : MonoBehaviour {
    public string databaseURL = "https://7913dc07-9b69-4a17-8329-cb43b00bbd14-bluemix:5d53ba229591931afc62e00011b05687a17ce40012615c32a826b8219f106d2d@7913dc07-9b69-4a17-8329-cb43b00bbd14-bluemix.cloudant.com/";
    public string projectName = "broadway61/";

    public WebRequest req;

    void Start () {
        req = this.gameObject.AddComponent<WebRequest>();
    }

    public void GetAllAssetsTrigger()
    {
        string url = databaseURL + projectName + "_all_docs?include_docs=true";
        req.HTTPGETRequest(url, getAllAssetsHandler);
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
    }

    private SyncedAsset[] saArray;

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
        string url = databaseURL + projectName + "_bulk_docs";
        var headers = new Dictionary<string, string>();
        headers.Add("X-HTTP-Method-Override", "POST");
        headers.Add("Content-Type", "application/json");
        string postData = "{\"docs\":[";
        saArray = FindObjectsOfType<SyncedAsset>();
        for(int i=0; i < saArray.Length; i++)
        {
            JSONNode n = JSON.Parse(JsonUtility.ToJson(saArray[i]));
            if (n["_id"] == null || n["_rev"] == null) {
                n.Remove("_id");
                n.Remove("_rev");
                    }
            postData += n.ToString();
            if (i != saArray.Length - 1) postData += ",";
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
        string url = databaseURL + projectName + "_bulk_docs";
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
