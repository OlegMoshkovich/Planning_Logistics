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
    private MqttClient client2;
    private MqttClient clientBlueMix;
    public List<Tag> listOfQTrackTags = new List<Tag>();
    public List<HCSTag> listOfHCSTags = new List<HCSTag>();

    public static mqttManager main;
	// Use this for initialization
	void Start () {
        main = this;
        Debug.Log("start");
		// create client instance 
		client = new MqttClient(IPAddress.Parse("137.135.91.79"),1883 , false , null );
        client2 = new MqttClient(IPAddress.Parse("137.135.91.79"), 1883, false, null);
        clientBlueMix = new MqttClient("tmsmv4.messaging.internetofthings.ibmcloud.com", 1883, false, null);
        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        client2.MqttMsgPublishReceived += client2_MqttMsgPublishReceived;
        clientBlueMix.MqttMsgPublishReceived += clientBlueMix_MqttMsgPusblishReceived;

        string clientId = Guid.NewGuid().ToString(); 
		client.Connect(clientId);
        //client2.Connect(clientId);
        //clientBlueMix.Connect( clientID, username, password)
        clientBlueMix.Connect("a: tmsmv4:493fpablo", "a-tmsmv4-ubooo5duzq", "7mAjKMKu@aWG0c4f08");

        // Q-Track Subscribe
        client.Subscribe(new string[] { "qtrack/+" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client2.Subscribe(new string[] { "V1/vest/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        //topic to publish {"Alarm": 0}
        clientBlueMix.Subscribe(new string[] { "iot-2/type/HCSTag/id/493f/cmd/calib/fml/json" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });


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
            foreach (Tag tag in listOfQTrackTags){
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
                listOfQTrackTags.Add(newTag);
            }
        }
        catch (Exception error)
        {
            Debug.LogError("error" + error);
        }
    }
    void client2_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {

        string message = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received: " + message);
        string mac = e.Topic.ToString().Split('/')[2];
        string messageType = e.Topic.ToString().Split('/')[3];
        Debug.Log("Topic: " + mac);
        Debug.Log("messageType: " + messageType);
        try
        {
            HCSTag incomingTag = new HCSTag();
            incomingTag = JsonUtility.FromJson<HCSTag>(message);
            incomingTag.macAddress = mac;
            bool createNewTag = true;
            for(int i=0; i<listOfHCSTags.Count;i++)
            {
                if (listOfHCSTags[i].macAddress == mac)
                {
                    switch (messageType)
                    {
                        case "orin":
                            listOfHCSTags[i].ori = incomingTag.ori;
                            listOfHCSTags[i].gyr = incomingTag.gyr;
                            break;
                        case "envi":
                            listOfHCSTags[i].light = incomingTag.light;
                            listOfHCSTags[i].uvi = incomingTag.uvi;
                            break;
                        case "time":
                            break;
                    }
                    listOfHCSTags[i] = incomingTag;
                    createNewTag = false;
                }
            }
            if (createNewTag == true)
            {
                listOfHCSTags.Add(incomingTag);
            }
        }
        catch (Exception error)
        {
            Debug.LogError("error" + error);
        }
    }
    void clientBlueMix_MqttMsgPusblishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("BLUEMIX received");
    }
    void OnGUI(){
		/*if ( GUI.Button (new Rect (20,40,80,20), "Send Alert")) {
			Debug.Log("sending...");
			client.Publish("alerts", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
			Debug.Log("sent");
		}*/
	}

	// Update is called once per frame
	void Update () {


	}
}
