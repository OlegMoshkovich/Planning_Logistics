using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorkerTelemetryCanvas : MonoBehaviour {
    public LineChart _LineChart = null;
    private float[][] data;
    private int t = 0 ; //Used to measure point in data to update
    private WorkerTelemetry workerTelemetry = null;
    public Text nameText;
    public Text tradeText; 
    public GameObject OrientationObject;
    public Text temperatureText;
    public Text pressureText;
    public Text humidity;
    public Text HCSTagIdText;
    public Text QTrackText;
    public GameObject panel;
    private GameObject worker;
    public static WorkerTelemetryCanvas main;


    // Use this for initialization
    private void OnEnable()
    {
        
    }
    public void activatePanel(GameObject selectedWorker)
    {
        panel.SetActive(true);
        worker = selectedWorker;
        workerTelemetry = worker.GetComponent<WorkerTelemetry>();
    }
    private void Awake()
    {
        main = this;
    }
    void Start () {
        
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
        if (value) mqttManager.main.ActivateBadBend(workerTelemetry.macAddress);
        else mqttManager.main.DeactivateBadBend(workerTelemetry.macAddress);
    }
	
	void Update () {
        if (panel.active)
        {

            if (t < data[0].Length - 1) t += 1;
            else t = 0;
            if (mqttManager.main.listOfHCSTags[0].ori[0] != null)
            {
                data[0][t] = Mathf.Sqrt(Mathf.Pow(mqttManager.main.listOfHCSTags[0].acc[0], 2) + Mathf.Pow(mqttManager.main.listOfHCSTags[0].acc[1], 2) + Mathf.Pow(mqttManager.main.listOfHCSTags[0].acc[2], 2));

            }

            if (_LineChart != null)
            {
                _LineChart.UpdateData(data);
            }
            temperatureText.text = mqttManager.main.listOfHCSTags[0].tmp.ToString() + " C";
            pressureText.text = mqttManager.main.listOfHCSTags[0].bmp.ToString() + " Pa";
            humidity.text = mqttManager.main.listOfHCSTags[0].hum.ToString() + " %";
            nameText.text = worker.name;
            HCSTagIdText.text = workerTelemetry.macAddress;
            QTrackText.text = worker.GetComponent<WorkerTagMovement>().frequency.ToString();
            OrientationObject.transform.rotation = Quaternion.Slerp(OrientationObject.transform.rotation, Quaternion.Euler(mqttManager.main.listOfHCSTags[0].ori[0], mqttManager.main.listOfHCSTags[0].ori[1], mqttManager.main.listOfHCSTags[0].ori[2]), 2 * Time.deltaTime);
        }

    }
}
