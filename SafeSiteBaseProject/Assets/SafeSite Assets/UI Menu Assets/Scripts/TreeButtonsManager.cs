using UnityEngine;
using System.Collections;
using Battlehub.UIControls;
using UnityEngine.EventSystems;

public class TreeButtonsManager : MonoBehaviour
{

    public void CreateFolder()
    {
        GameObject newFolder = new GameObject();
        newFolder.name = "New Folder";
        newFolder.AddComponent<Folder>();
        TreeViewManager.main.TreeView.Add(newFolder);
    }
}
