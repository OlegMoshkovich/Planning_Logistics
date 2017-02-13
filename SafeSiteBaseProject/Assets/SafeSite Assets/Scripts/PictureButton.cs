using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Windows;
using System.IO;

public class PictureButton : MonoBehaviour {
    public RawImage imagePlaceholder;
    public Button openTakePictureButton;
    public GameObject takePicturePanel;
    public RawImage cameraRawImage;
    public SyncedHazard synchedHazard;

    private WebCamTexture _camTexture;
    private Texture2D unityScreencapture;
    private Texture2D cameraPicture;
    

    // Use this for initialization
    void Start () {
        //Check if device has camera
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        Debug.Log("Cameras Detected: " + WebCamTexture.devices.Length);
        if (WebCamTexture.devices.Length > 0)
        {
            openTakePictureButton.interactable = true;
        }
        else
        {
            openTakePictureButton.interactable = false;
        }

        //Load Picture
        if(synchedHazard.imageURL != null && (ObjectStorageAPI.main != null)) ObjectStorageAPI.main.loadImageTrigger(synchedHazard.imageURL, onImageDownloadComplete);
            else Debug.Log("Missing Image URL or ObjectStorageAPI Manager");
    }

    public void onImageDownloadComplete()
    {
        imagePlaceholder.texture = ObjectStorageAPI.main.req.download.texture;
    }

    public void onOpenTakePictureHandler()
    {
        _camTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        takePicturePanel.SetActive(true);
        cameraRawImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        cameraRawImage.material.mainTexture = _camTexture;
        _camTexture.Play();
        float rotationY = _camTexture.videoVerticallyMirrored ? -1.0f : 1.0f;
        cameraRawImage.transform.localScale = new Vector3(1.0f, rotationY, 0.0f);
    }

    public void OnTakePictureHandler()
    {
        Debug.Log(_camTexture.width.ToString() + " " + _camTexture.height.ToString());
        cameraPicture = new Texture2D(_camTexture.width, _camTexture.height, TextureFormat.ARGB32, false);
        cameraPicture.SetPixels(_camTexture.GetPixels());
        cameraPicture.Apply();
        imagePlaceholder.texture = cameraPicture;
        takePicturePanel.SetActive(false);
        _camTexture.Stop();
        byte[] bytes = cameraPicture.EncodeToPNG();
        synchedHazard.imageURL = synchedHazard.name + System.DateTime.Now.ToString("MM-dd-yyyy--hh-mm-ss")+".png" ;
        StartCoroutine(ObjectStorageAPI.main.saveImage(bytes, synchedHazard.imageURL));
        
    }

}
