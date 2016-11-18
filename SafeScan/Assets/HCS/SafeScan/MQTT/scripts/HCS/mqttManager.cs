using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

public class mqttManager : MonoBehaviour {
    public bool convertUnitsToMeters = true;
	private MqttClient client;
    public List<Tag> listOfTags = new List<Tag>();
	// Use this for initialization
	void Start () {
		// create client instance 
		client = new MqttClient(IPAddress.Parse("137.135.91.79"),1883 , false , null ); 
		
		// register to message received 
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		string clientId = Guid.NewGuid().ToString(); 
		client.Connect(clientId); 
		
		// subscribe to the topic "/home/temperature" with QoS 2 
		client.Subscribe(new string[] { "qtrack/+" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        Debug.Log("start");

	}
	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{

        string message = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received: " + message);
        int frequency = int.Parse(e.Topic.ToString().Split('/')[1]);

        try
        {
            Tag incomingTag = new Tag();
            incomingTag = JsonUtility.FromJson<Tag>(message);
            incomingTag.Frequency = frequency;
            Debug.Log(incomingTag);
            Debug.Log("X: " + incomingTag.X + "Y: " + incomingTag.Y + " Frequency: " + frequency.ToString());
            bool createNewTag = true;
            foreach (Tag tag in listOfTags){
                if (tag.Frequency == frequency)
                {
                    tag.Frequency = frequency;               
                    tag.X = incomingTag.X;
                    tag.Y = incomingTag.Y;
                    if (convertUnitsToMeters) {
                        tag.X = tag.X * 10 / 36;
                        tag.Y = tag.Y * 10 / 36;
                    }
                    createNewTag = false;
                }
            }
            if(createNewTag == true)
            {
                Tag newTag = incomingTag;
                listOfTags.Add(newTag);
            }
        }
        catch (Exception error)
        {
            Debug.LogError("error" + error);
        }
    } 

	void OnGUI(){
		if ( GUI.Button (new Rect (20,40,80,20), "Send Alert")) {
			Debug.Log("sending...");
			client.Publish("alerts", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
			Debug.Log("sent");
		}
	}
	// Update is called once per frame
	void Update () {



	}
}
