using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WebRequest : MonoBehaviour {

    public bool processing = false;
    const float WWW_TIMEOUT = 300f;
    const float WWW_WAIT = 0.166f;

    public WWW download;

    public delegate void WebRequestEvent();
    public WebRequestEvent onProcessingFinished;


    private void onProcessingFinishedHandler()
    {
        Debug.Log("Finished Processing " + download.text);
    }

    private IEnumerator GETRequest(string url)
    {
        Debug.Log("Requesting: " + url);
        processing = true;
        download = new WWW(url);
        var timeOutIndex = 0;
        while (!download.isDone && (timeOutIndex < WWW_TIMEOUT) && string.IsNullOrEmpty(download.error))
        {
            timeOutIndex++;
            yield return new WaitForSeconds(WWW_WAIT);
        }
        processing = false;
        if(onProcessingFinished != null) onProcessingFinished();
    }
    private IEnumerator GETRequestWithHeaders(string url, Dictionary<string, string> headers)
    {
        Debug.Log("Requesting: " + url);
        processing = true;
        download = new WWW(url, null, headers);
        var timeOutIndex = 0;
        while (!download.isDone && (timeOutIndex < WWW_TIMEOUT) && string.IsNullOrEmpty(download.error))
        {
            timeOutIndex++;
            yield return new WaitForSeconds(WWW_WAIT);
        }
        processing = false;
        if (onProcessingFinished != null) onProcessingFinished();
    }
    private IEnumerator POSTRequest(string url, byte[] postData, Dictionary<string, string> headers )
    {
        processing = true;
        download = new WWW(url, postData, headers);
        var timeOutIndex = 0;
        while (!download.isDone && (timeOutIndex < WWW_TIMEOUT) && string.IsNullOrEmpty(download.error))
        {
            timeOutIndex++;
            yield return new WaitForSeconds(WWW_WAIT);
        }
        processing = false;
        if (onProcessingFinished != null) onProcessingFinished();
    }

    
   
    public void HTTPGETRequest(string url, WebRequestEvent callbackFunction, Dictionary<string, string> headers = null)
    {
        if (processing)
        {
            Debug.LogError("Already processing request");
            return;
        }
        else
        {
            onProcessingFinished = callbackFunction;
#if UNITY_EDITOR
            Debug.Log("Sending GET Request to:  " + url);
#endif
            if(headers == null) StartCoroutine(GETRequest(url));
            else StartCoroutine(GETRequestWithHeaders(url, headers));
        }
    }

    public void HTTPRequest(string url, WebRequestEvent callbackFunction, byte[] postData, Dictionary<string, string> headers)
    {
        if (processing)
        {
            Debug.Log("Already processing request");
            return;
        }
        else
        {
            onProcessingFinished = callbackFunction;
#if UNITY_EDITOR
            Debug.Log("Sending HTTP Request with data: " + postData.ToString());
#endif
            StartCoroutine(POSTRequest(url, postData, headers));
        }   
    }
    public void HTTPPUTRequest(string url, WebRequestEvent callbackFunction, byte[] postData)
    {
        var headers = new Dictionary<string, string>();
        headers.Add("X-HTTP-Method-Override", "PUT");
        if (processing)
        {
            Debug.LogError("Already processing request");
            return;
        }
        else
        {
            onProcessingFinished = callbackFunction;
#if UNITY_EDITOR
            Debug.Log("Sending HTTP Request with data: " + postData.ToString());
#endif
            StartCoroutine(POSTRequest(url, postData, headers));
        }
    }
    public void HTTPPOSTRequest(string url, WebRequestEvent callbackFunction, byte[] postData)
    {
        var headers = new Dictionary<string, string>();
        headers.Add("X-HTTP-Method-Override", "POST");
        if (processing)
        {
            Debug.LogError("Already processing request");
            return;
        }
        else
        {
            onProcessingFinished = callbackFunction;
#if UNITY_EDITOR
            Debug.Log("Sending HTTP Request with data: " + postData.ToString());
#endif
            StartCoroutine(POSTRequest(url, postData, headers));
        }
    }





}
