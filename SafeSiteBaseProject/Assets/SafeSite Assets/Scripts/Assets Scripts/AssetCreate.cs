using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetCreate : MonoBehaviour {
    public Texture2D cursorIcon = null;
	public AssetType assetType; //Asset Type is defined in SynchedAsset

    public void createAsset(){
		AssetManager.main.createNewAsset (assetType, cursorIcon);
	}

}


