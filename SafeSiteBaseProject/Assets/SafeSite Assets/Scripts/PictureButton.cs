using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PictureButton : MonoBehaviour {
    private WebCamTexture _camTexture; 

    // Use this for initialization
    void Start () {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (WebCamTexture.devices.Length > 0)
        {
            this.gameObject.GetComponent<Button>().interactable = true;
            _camTexture = new WebCamTexture();
            _camTexture.Play();
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }    
    }
	
    public void OnButtonClickHandler()
    {
        Texture2D snap = new Texture2D(_camTexture.width, _camTexture.height);
        snap.SetPixels(_camTexture.GetPixels());
        snap.Apply();
        this.gameObject.GetComponentInChildren<RawImage>().texture = snap;
    }

}
