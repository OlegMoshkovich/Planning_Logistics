using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorkerInfoCanvas : MonoBehaviour {
    public GameObject worker;
    public Text nameText;
    public Text tradeText;

    public void OnClick_Handler()
    {
        WorkerTelemetryCanvas.main.activatePanel(worker);
        
    }
    private void OnEnable()
    {
        nameText.text = worker.name;
    }

}
