using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HazardStatusChange : MonoBehaviour {

  public void OnStatusChangeHandler(int val) {
        Image img = GetComponent<Image>();
        Color[] colors = { Color.red,  Color.white, Color.green};
        if (val < colors.Length && val >= 0) img.color = colors[val];
    }
}


