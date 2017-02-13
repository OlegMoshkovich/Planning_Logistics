using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ObjectStorageAPI : MonoBehaviour {
    public string url = "https://dal.objectstorage.open.softlayer.com/v1/AUTH_6b5f695362b44c22822db23d3cc591e1";
    public string container = "broadway61/";
    public string authToken = "gAAAAABYodco1ZJtl0KVNDpRQo2b3_IMBzE-KVox_kkWBNm4QP6dPqYDBtP2kjj67SHwXifN97-V4jWVS9Yf4ADVqdCECkVrxOAGF08h6K2mqgwf5dwLgbbe52L1YhzMcN3xDt1UxbxYE5VvZztnl8UfNfHgRXwOJ5T8K6Tj4PUPurKr8wcAhRk";
    public WebRequest req;
    public static ObjectStorageAPI main;

    private Dictionary<string, string> headers;

    protected void Awake()
    {
        main = this;
        req = this.gameObject.AddComponent<WebRequest>();
        headers = new Dictionary<string, string>();
        headers.Add("X-Auth-Token", authToken);


    }

	public void loadImageTrigger(string imageURL, WebRequest.WebRequestEvent onLoadImageComplete )
    { 
        req.HTTPRequest(url+"/"+container+imageURL, onLoadImageComplete, null, headers);
    }

    

    public IEnumerator saveImage(byte[] image, string imageURL)
    {
        Debug.Log("Start Save Image: " + imageURL);
        //Set up PUT REQUEST
        var unityWebRequest = new UnityWebRequest(url + "/" + container + imageURL);
        unityWebRequest.SetRequestHeader("X-Auth-Token", authToken);
        unityWebRequest.SetRequestHeader("Content-Type", "image/png");
        unityWebRequest.method = "PUT";
        UploadHandler uploadHandler = new UploadHandlerRaw(image);     
        unityWebRequest.uploadHandler = uploadHandler;
        yield return unityWebRequest.Send();
        if (unityWebRequest.isError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log("Upload Image Response Code : " + unityWebRequest.responseCode.ToString());
        }
    }


}
