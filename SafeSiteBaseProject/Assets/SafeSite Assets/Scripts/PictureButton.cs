using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PictureButton : MonoBehaviour {
    public RawImage imagePlaceholder;
    public Button openTakePictureButton;
    public GameObject takePicturePanel;
    public RawImage cameraRawImage;

    private WebCamTexture _camTexture;
    private Texture2D unityScreencapture;
    private Texture2D cameraPicture;

    // Use this for initialization
    void Start () {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        Debug.Log(WebCamTexture.devices.Length);
        if (WebCamTexture.devices.Length > 0)
        {
            Debug.Log("onOpenTake");
            openTakePictureButton.interactable = true;
            
        }
        else
        {
            openTakePictureButton.interactable = false;
        }

    }
    public void onOpenTakePictureHandler()
    {
        _camTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        takePicturePanel.SetActive(true);
        cameraRawImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        cameraRawImage.material.mainTexture = _camTexture;
        _camTexture.Play();
    }

    public void OnTakePictureHandler()
    {
        Debug.Log(_camTexture.width.ToString() + " " + _camTexture.height.ToString());
        cameraPicture = new Texture2D(_camTexture.width, _camTexture.height, TextureFormat.RGB24, false);
        cameraPicture.SetPixels(_camTexture.GetPixels());
        cameraPicture.Apply();
        imagePlaceholder.texture = cameraPicture;
        byte[] bytes = cameraPicture.EncodeToPNG();
        Debug.Log(JsonUtility.ToJson(bytes).ToString());
        takePicturePanel.SetActive(false);
        _camTexture.Stop();
#if UNITY_EDITOR
        // For testing purposes, also write to a file in the project folder
        //File.WriteAllBytes(Application.dataPath + "SavedScreen.png", bytes);
#endif
    }

}
