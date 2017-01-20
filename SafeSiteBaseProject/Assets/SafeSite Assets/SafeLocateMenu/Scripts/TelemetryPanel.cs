using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TelemetryPanel : MonoBehaviour {
    public LineChart _LineChart = null;
    private float[][] data;
    private int t = 0 ; //Used to measure point in data to update
    public HCSTelemetry hcsTelemetry;
    public Toggle badBend;
    public GameObject OrientationObject;
    public Text temperatureText;
    public Text pressureText;
    public Text humidity;
    public Text HCSTagIdText;

    void Start () {
        hcsTelemetry = AssetPanel.main.selectedAsset.GetComponent<HCSTelemetry>();
        //Set up graph
        int max = 200;
        t = 0;
        data = new float[1][];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new float[max];
            for (int j = 0; j < max; j++)
            {
                data[i][j] = 0;
            }
        }
    }
	public void OnBadBendToggleChange_Handler(bool value)
    {
        if (value) mqttManager.main.ActivateBadBend(hcsTelemetry.macAddress);
        else mqttManager.main.DeactivateBadBend(hcsTelemetry.macAddress);
        hcsTelemetry.badBendActivated = badBend.isOn;
    }

    public void OnClickAlertButtonHandler()
    {
        mqttManager.main.ActivateAlarm(hcsTelemetry.macAddress);
    }

    public void DeleteTelemetry()
    {
        AssetPanel.main.telemetryPanel.active = false;
        DestroyImmediate(AssetPanel.main.selectedAsset.GetComponent<HCSTelemetry>());
        AssetPanel.main.addTelemetryPanel.active = true;
    }
	
	void Update () {
        if (AssetPanel.main.selectedAsset.GetComponent<HCSTelemetry>().macAddress != "")
        {
            HCSTag tag = mqttManager.main.listOfHCSTags[hcsTelemetry.macAddress];
            if (t < data[0].Length - 1) t += 1;
            else t = 0;
            if (tag.ori[0] != null)
            {
                data[0][t] = Mathf.Sqrt(Mathf.Pow(tag.acc[0], 2) + Mathf.Pow(tag.acc[1], 2) + Mathf.Pow(tag.acc[2], 2));

            }

            if (_LineChart != null)
            {
                _LineChart.UpdateData(data);
            }

            //Update Data
            badBend.isOn = AssetPanel.main.selectedAsset.GetComponent<HCSTelemetry>().badBendActivated;
            temperatureText.text = tag.tmp.ToString() + " C";
            pressureText.text = tag.bmp.ToString() + " Pa";
            humidity.text = tag.hum.ToString() + " %";
            HCSTagIdText.text = tag.macAddress;
            OrientationObject.transform.rotation = Quaternion.Slerp(OrientationObject.transform.rotation, Quaternion.Euler(tag.ori[0], tag.ori[1], tag.ori[2]), 2 * Time.deltaTime);

        }

    }
}
