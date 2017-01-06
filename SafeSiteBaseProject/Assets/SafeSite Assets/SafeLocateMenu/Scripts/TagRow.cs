using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TagRow : MonoBehaviour {
    public Text NameText;
    public Text QTrackKHzText;
    public Text HCSTagIDText;

    public int index = 0;

    public void OnQTrackTextSubmit(int value)
    {
        WorkerManager.main.listOfWorkers[index].GetComponent<WorkerTagMovement>().frequency = value;
    }
}
