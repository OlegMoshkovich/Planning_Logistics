using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetCreate : MonoBehaviour {
    public Texture2D cursorIcon = null;
	public string assetType;

	public void createAsset(){
		AssetManager.main.createNewAsset (assetType, cursorIcon);
	}

}


