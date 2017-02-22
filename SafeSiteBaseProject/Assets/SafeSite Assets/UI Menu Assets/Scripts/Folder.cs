using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Folder : MonoBehaviour {

    public void OnEndEdit()
    {
        this.GetComponentInChildren<Text>().text = this.GetComponentInChildren<InputField>().text;
        Destroy(this.GetComponentInChildren<InputField>());
    }
}
