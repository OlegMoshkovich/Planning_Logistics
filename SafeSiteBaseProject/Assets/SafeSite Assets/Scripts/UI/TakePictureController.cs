using UnityEngine;
using UnityEngine.UI;


public class TakePictureController : MonoBehaviour {
    public RawImage cameraPhotoPlaceholder;
    public Button openTakePictureButton;
    public GameObject takePicturePanel;
    public RawImage cameraRawImage;
    public SyncedHazard synchedHazard;
    public Text imageName;

    private WebCamTexture _camTexture;
    private Texture2D unityScreencapture;
    private Texture2D cameraPicture;
    
   
    // Use this for initialization
    void Start () {
        //Check fields are defined
        if (cameraPhotoPlaceholder == null || openTakePictureButton == null ||
            takePicturePanel == null || cameraRawImage == null || synchedHazard == null || imageName == null) Debug.LogError("Missing Defined Field");
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
        if(synchedHazard.sa_imageURL.Count>0 && (ObjectStorageAPI.main != null)) ObjectStorageAPI.main.loadImageTrigger(synchedHazard.sa_imageURL[synchedHazard.sa_imageURL.Count-1], onImageDownloadComplete);
            else Debug.Log("Missing Image URL or ObjectStorageAPI Manager");
    }

    public void onImageDownloadComplete()
    {
        cameraPhotoPlaceholder.texture = ObjectStorageAPI.main.req.download.texture;
    }

    public void onOpenTakePictureHandler()
    {
        _camTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        takePicturePanel.SetActive(true);
        cameraRawImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        cameraRawImage.material.mainTexture = _camTexture;
        _camTexture.Play();
        //InvokeRepeating("CheckCameraRotation", 0, 0.5f);
    }

    private void CheckCameraRotation()
    {
        float rotationY = _camTexture.videoVerticallyMirrored ? -1.0f : 1.0f;
        cameraRawImage.transform.localScale = new Vector3(1.0f, rotationY, 0.0f);
    }

    public void OnTakePictureHandler()
    {
        //CancelInvoke("CheckCameraRotation");
        Debug.Log(_camTexture.width.ToString() + " " + _camTexture.height.ToString());
        cameraPicture = new Texture2D(_camTexture.width, _camTexture.height, TextureFormat.ARGB32, false);
        cameraPicture.SetPixels(_camTexture.GetPixels());
        cameraPicture.Apply();
        cameraPhotoPlaceholder.texture = cameraPicture;
        takePicturePanel.SetActive(false);
        _camTexture.Stop();
        byte[] bytes = cameraPicture.EncodeToPNG();
        string newImageURL = synchedHazard.name + SyncedAsset.GetUTCTimeStamp() + ".png";
        synchedHazard.sa_imageURL.Add(newImageURL);
        StartCoroutine(ObjectStorageAPI.main.saveImage(bytes, newImageURL));
        
    }

}
