using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Net;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour  {

    public delegate void UnityWebRequestEvent(UnityWebRequest req);
    public UnityWebRequestEvent OnUnityWebRequestDone;

    public void SendUnityWebRequest(UnityWebRequest req)
    {
        StartCoroutine(UnityWebRequestRoutine(req));
    }
    public void SendUnityWebRequest(UnityWebRequest req, UnityWebRequestEvent callbackFunction)
    {
            OnUnityWebRequestDone = callbackFunction;
            SendUnityWebRequest(req);
    }
    
    public IEnumerator UnityWebRequestRoutine (UnityWebRequest req)
    {
        Debug.Log("Requesting: " + req.url);
        yield return req.Send();
        if (req.isError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log("Server Response Code : " + req.responseCode);
            if (OnUnityWebRequestDone != null) OnUnityWebRequestDone(req);
        }
    }
}
