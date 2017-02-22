using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Hazard_Status_Change : MonoBehaviour {

  public void onStatusChangeHandler(int val) {
        Image img = GetComponent<Image>();

        if (val == 0) img.color = Color.red;

        if (val == 1) img.color = Color.white;

        if (val == 2)img.color = Color.green;
    }
}


