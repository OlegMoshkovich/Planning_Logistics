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
    public Dictionary<String, Tag> listOfQTrackTags = new Dictionary<String, Tag>();
    public Dictionary<String, HCSTag> listOfHCSTags = new Dictionary<String, HCSTag>();
    public List<string> incomingLog = new List<string>();
    public List<string> outgoingLog = new List<string>();

    public static mqttManager main;
    private void Awake()
    {
        main = this;
    }
    // Use this for initialization
    void Start () {
        
        Debug.Log("start");
		
        // create client instance 
		client = new MqttClient(IPAddress.Parse("137.135.91.79"),1883 , false , null );
        clientBlueMix = new MqttClient("tmsmv4.messaging.internetofthings.ibmcloud.com", 1883, false, null);
        
        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        clientBlueMix.MqttMsgPublishReceived += clientBlueMix_MqttMsgPusblishReceived;

        //Register Messages To IncomingLog
        client.MqttMsgPublishReceived += logIncomingData;
        clientBlueMix.MqttMsgPublishReceived += logIncomingData;

        //Register Messages to OutgoingLog
        client.MqttMsgPublished += logOutgoingData;
        clientBlueMix.MqttMsgPublished += logOutgoingData;

        try{
            string clientId = Guid.NewGuid().ToString();
            var QtrackStatus = client.Connect(clientId);
            Debug.Log("QTrack MQTT Client status: " + QtrackStatus.ToString());
        }
                catch (MqttCommunicationException e)
        {
            Debug.LogError(e.ToString());
        }

		
        //clientBlueMix.Connect( clientID, username, password)
        try
        {
            var blueMixStatus = clientBlueMix.Connect("a:tmsmv4:pablo", "a-tmsmv4-r2pl0u7lrz", "@BAr8DooQs(Eih0Ocb");
            Debug.Log("Bluemix connected + status: " + blueMixStatus.ToString());

        }
        catch (MqttCommunicationException e)
        {
            Debug.LogError(e.ToString());
        }


        client.Subscribe(new string[] { "qtrack/+" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        //clientBlueMix.Subscribe(new string[] { "iot-2/type/HCSTag/id/+/evt/+/fmt/json" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        clientBlueMix.Subscribe(new string[] { "iot-2/type/HCS_BLE_Tag/id/+/evt/+/fmt/json" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });


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
            if (listOfQTrackTags.ContainsKey(incomingTag.Frequency.ToString()))
            {
                listOfQTrackTags.Add(incomingTag.Frequency.ToString(), incomingTag);
            }
            else
            {
                listOfQTrackTags[incomingTag.Frequency.ToString()] = incomingTag;
            }

        }
        catch (Exception error)
        {
            Debug.LogError("error" + error);
        }
    }
    
    void clientBlueMix_MqttMsgPusblishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string message = System.Text.Encoding.UTF8.GetString(e.Message);
        string evt = e.Topic.ToString().Split('/')[6];
        string macAddress = e.Topic.ToString().Split('/')[4];
        try
        {
            HCSTag incomingTag = new HCSTag();
            incomingTag = JsonUtility.FromJson<HCSTag>(message);
            incomingTag.macAddress = macAddress;
            
            //IF tag exists, update it:
            if(listOfHCSTags.ContainsKey(macAddress))
                {
                    switch (evt)
                    {
                        case "orin":
                            listOfHCSTags[macAddress].ori = incomingTag.ori;
                            listOfHCSTags[macAddress].acc = incomingTag.acc;
                            listOfHCSTags[macAddress].gyr = incomingTag.gyr;
                            break;
                        case "envi":
                            listOfHCSTags[macAddress].lit = incomingTag.lit;
                            listOfHCSTags[macAddress].snd = incomingTag.snd;
                            listOfHCSTags[macAddress].hum = incomingTag.hum;
                            listOfHCSTags[macAddress].bmp = incomingTag.bmp;
                            listOfHCSTags[macAddress].uvi = incomingTag.uvi;
                            listOfHCSTags[macAddress].tmp = incomingTag.tmp;
                            listOfHCSTags[macAddress].alt = incomingTag.alt;
                            break;
                        case "meta":
                            listOfHCSTags[macAddress].bat = incomingTag.bat;
                            listOfHCSTags[macAddress].tim = incomingTag.tim;
                            break;

                    }
                }
            // If tag doesn't exist, create it:
           else
            {
                HCSTag newTag = incomingTag;
                listOfHCSTags.Add(incomingTag.macAddress, newTag);
            }
                
        }
        catch (Exception error)
        {
            Debug.Log("error" + error);
        }
    }

    void logOutgoingData(object sender, MqttMsgPublishedEventArgs e)
    {
        if (outgoingLog.Count > 100) outgoingLog = new List<string>();
        string message = "Sender: " + sender.ToString() + " Message: " + e.ToString();
        outgoingLog.Add(message);
        Debug.Log(e.ToString());
    }

    void logIncomingData(object sender, MqttMsgPublishEventArgs e)
    {
        if (incomingLog.Count > 100) incomingLog = new List<string>();
        string message = "Time :"+ DateTime.Now + "Topic: " + e.Topic;
        message += " Message: "+ System.Text.Encoding.UTF8.GetString(e.Message);
        incomingLog.Add(message);
    }

    public void ActivateAlarm(string macAddress)
    {
        string topic = "iot-2/type/HCSTag/id/" + macAddress + "/cmd/alert/fmt/json";
        clientBlueMix.Publish(topic, System.Text.Encoding.UTF8.GetBytes("{\"alarm\":1}"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
    }
    public void ActivateBadBend(string macAddress)
    {
        string topic = "iot-2/type/HCSTag/id/" + macAddress + "/cmd/alert/fmt/json";
        clientBlueMix.Publish(topic, System.Text.Encoding.UTF8.GetBytes("{\"bndbz\":1}"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
    }
    public void DeactivateBadBend(string macAddress)
    {
        string topic = "iot-2/type/HCSTag/id/" + macAddress + "/cmd/alert/fmt/json";
        clientBlueMix.Publish(topic, System.Text.Encoding.UTF8.GetBytes("{bndbz:0}"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
    }

    public void DeactivateAlarm(string macAddress)
    {
        string topic = "iot-2/type/HCSTag/id/" + macAddress + "/cmd/alert/fmt/json";
        clientBlueMix.Publish(topic, System.Text.Encoding.UTF8.GetBytes("{alarm:0}"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
    }

    public void ActivateAllAlarms()
    {
        foreach(KeyValuePair<string, HCSTag> item in listOfHCSTags)
        {
            ActivateAlarm(item.Value.macAddress.ToString());
        }
    }


}
