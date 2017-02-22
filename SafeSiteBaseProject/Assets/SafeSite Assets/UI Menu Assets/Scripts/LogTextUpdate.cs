using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogTextUpdate : MonoBehaviour {

    public bool showIncoming { get; set; }
    public bool showOutgoing { get; set; }
    private Text _logText;

    // Use this for initialization
    void Start () {
        showIncoming = true;
        showOutgoing = true;
        _logText = GetComponentInChildren<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        _logText.text = "";
        if (showIncoming)
        {
            foreach (string s in mqttManager.main.incomingLog)
            {
                _logText.text += s + "\n";
            }
        }
        if (showOutgoing)
        {
            foreach (string s in mqttManager.main.outgoingLog)
            {
                _logText.text += s + "\n";
            }
        }

    }
}
