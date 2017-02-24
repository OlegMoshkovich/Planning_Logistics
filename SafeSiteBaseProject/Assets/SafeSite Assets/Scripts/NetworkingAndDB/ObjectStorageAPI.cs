using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ObjectStorageAPI : MonoBehaviour {
    public string url = "https://dal.objectstorage.open.softlayer.com/v1/AUTH_6b5f695362b44c22822db23d3cc591e1";
    public Dictionary<string, string> container = new Dictionary<string, string>(){{"61Broadway", "broadway61/" },
                                                                                { "Queens Plaza", "queens_plaza/"},
                                                                                { "DGH Lincoln Center", "lincoln_center/"},
                                                                                { "VR_Forklift_06", "forkliftvr/"}};
    private string authToken;
    public WebRequest req;
    public static ObjectStorageAPI main;

    public string user_id = "2ee9cff5e8af45748c04df0907a83451";
    public string user_password = "bBRi(AT99WLjHTn=";
    public string project_id = "6b5f695362b44c22822db23d3cc591e1";

    private Dictionary<string, string> headers;

    protected void Awake()
    {
        main = this;
        req = gameObject.AddComponent<WebRequest>();
        headers = new Dictionary<string, string>();
        headers.Add("X-Auth-Token", authToken);

        StartCoroutine(GetAuthToken());
    }

	public void loadImageTrigger(string imageURL, WebRequest.WebRequestEvent onLoadImageComplete )
    {
        headers["X-Auth-Token"] = authToken;
        req.HTTPRequest(url+"/"+container[SceneManager.GetActiveScene().name] +imageURL, onLoadImageComplete, null, headers);
    }
    public IEnumerator GetAuthToken()
    {
        string postData = "{\"auth\": {\"identity\": {\"methods\": [\"password\"],\"password\": {\"user\": {\"id\": \"" + user_id + "\", \"password\": \"" + user_password + "\"}} },\"scope\": { \"project\": {\"id\": \"" + project_id + "\" } } } }";
        var unityWebRequest = new UnityWebRequest("https://identity.open.softlayer.com/v3/auth/tokens", UnityWebRequest.kHttpVerbPOST)
                                                {
                                                    uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(postData))
                                                };
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Send Request with Data: " + postData);
        yield return unityWebRequest.Send();
        if (unityWebRequest.isError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log("Upload Image Response Code : " + unityWebRequest.responseCode);
            authToken = unityWebRequest.GetResponseHeader("X-Subject-Token");
        }
    }
    

    public IEnumerator saveImage(byte[] image, string imageURL)
    {
        Debug.Log("Start Save Image: " + imageURL);
        //Set up PUT REQUEST
        var unityWebRequest = new UnityWebRequest(url + "/" + container[SceneManager.GetActiveScene().name] + imageURL, UnityWebRequest.kHttpVerbPUT);
        unityWebRequest.SetRequestHeader("X-Auth-Token", authToken);
        unityWebRequest.SetRequestHeader("Content-Type", "image/png");
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
