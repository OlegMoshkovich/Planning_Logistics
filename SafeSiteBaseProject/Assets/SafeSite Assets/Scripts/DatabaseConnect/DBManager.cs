using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class DBManager : MonoBehaviour {
    public string databaseURL = "https://7913dc07-9b69-4a17-8329-cb43b00bbd14-bluemix:5d53ba229591931afc62e00011b05687a17ce40012615c32a826b8219f106d2d@7913dc07-9b69-4a17-8329-cb43b00bbd14-bluemix.cloudant.com/";
    public string projectName = "broadway61/";

    public WebRequest req;

	void Start () {
        req = this.gameObject.AddComponent<WebRequest>();
        GetAllAssetsTrigger();
    }

    public void GetAllAssetsTrigger()
    {
        string url = databaseURL + projectName + "_all_docs?include_docs=true";
        req.HTTPRequest(url, getAllAssetsHandler);
    }

    public void getAllAssetsHandler()
    {
        var n = JSON.Parse(req.download.text);
        for(int i =0; i<n["rows"].Count; i++)
        {
            Debug.Log(n["rows"][i]["doc"]["asset"].ToString());
            Debug.Log(n["rows"][i]["doc"]["rtls"].ToString());
        }
        
    }

    private void updateAssetsTrigger()
    {
        string url = databaseURL + projectName + "5b0b6c8e032c56be3d5eefbe87512745";
        var headers = new Dictionary<string, string>();
        headers.Add("X-HTTP-Method-Override", "PUT");
        headers.Add("Content-Type", "application /json");
        req.HTTPRequest(url, updateAssetsHandler, System.Text.Encoding.ASCII.GetBytes(""), headers);
    }

    private void updateAssetsHandler()
    {
        Debug.Log("updateAssetsHandler : " + req.download.text);
    }
}
