using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PictureButton : MonoBehaviour {
    public RawImage imagePlaceholder;
    public Button openTakePictureButton;
    public Canvas takePictureCanvas;
    public RawImage cameraRawImage;

    private WebCamTexture _camTexture;
    private Texture2D unityScreencapture;
    private Texture2D cameraPicture;

    // Use this for initialization
    void Start () {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (WebCamTexture.devices.Length > 0)
        {
            openTakePictureButton.interactable = true;
            _camTexture = new WebCamTexture(WebCamTexture.devices[0].name);
            _camTexture.Play();
        }
        else
        {
            openTakePictureButton.interactable = false;
        }    
    }
    public void onOpenTakePictureHandler()
    {
        Debug.Log("onOpenTake");
        takePictureCanvas.enabled = true;
        takePictureCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        cameraRawImage.texture = _camTexture;
    }

    public void OnTakePictureHandler()
    {
        cameraPicture = new Texture2D(_camTexture.width, _camTexture.height, TextureFormat.RGB24, false);
        cameraPicture.SetPixels(_camTexture.GetPixels());
        cameraPicture.Apply();
        this.gameObject.GetComponentInChildren<RawImage>().texture = cameraPicture;
        byte[] bytes = cameraPicture.EncodeToPNG();
        takePictureCanvas.enabled = false;
#if UNITY_EDITOR
        // For testing purposes, also write to a file in the project folder
        //File.WriteAllBytes(Application.dataPath + "SavedScreen.png", bytes);
#endif
    }

}
