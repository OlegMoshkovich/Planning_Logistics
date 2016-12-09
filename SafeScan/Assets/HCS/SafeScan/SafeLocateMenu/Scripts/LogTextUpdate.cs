using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogTextUpdate : MonoBehaviour {

    public bool showIncoming { get; set; }
    public bool showOutgoing { get; set; }

    // Use this for initialization
    void Start () {
        showIncoming = true;
        showOutgoing = true;

    }
	
	// Update is called once per frame
	void Update () {
        this.gameObject.GetComponentInChildren<Text>().text = "";
        if (showIncoming)
        {
            foreach (string s in mqttManager.main.incomingLog)
            {
                this.gameObject.GetComponentInChildren<Text>().text += s;
                this.gameObject.GetComponentInChildren<Text>().text += "\n";
            }
        }
        if (showOutgoing)
        {
            foreach (string s in mqttManager.main.outgoingLog)
            {
                this.gameObject.GetComponentInChildren<Text>().text += s;
                this.gameObject.GetComponentInChildren<Text>().text += "\n";
            }
        }

    }
}
