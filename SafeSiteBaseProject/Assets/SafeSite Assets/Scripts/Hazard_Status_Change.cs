using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Hazard_Status_Change : MonoBehaviour {
    

        public void onStatusChangeHandler(int val)
    {

        if (val == 0)
             {
                this.GetComponent<Image>().color = Color.red;
             }

        if (val == 1)
             {
                 this.GetComponent<Image>().color = Color.white;
             }

       if (val == 2)
             {
                 this.GetComponent<Image>().color = Color.green;
             }
    }


}


