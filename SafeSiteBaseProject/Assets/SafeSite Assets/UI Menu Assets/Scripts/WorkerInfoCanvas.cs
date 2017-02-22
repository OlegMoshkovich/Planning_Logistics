using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//This class is deprecated
public class WorkerInfoCanvas : MonoBehaviour {
    public GameObject worker;
    public Text nameText;
    public Text tradeText;

    private void OnEnable()
    {
        nameText.text = worker.name;
    }

}
