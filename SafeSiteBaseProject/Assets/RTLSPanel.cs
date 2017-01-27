using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RTLSPanel : MonoBehaviour {
    public RTLSMovement rtls;

    public Text rtlsId;
    public Text x_Value;
    public Text y_Value;
    public Text z_Value;

    private void Update()
    {
        //Debug.Log(rtls.frequency);
        //Debug.Log(mqttManager.main.listOfQTrackTags[rtls.frequency]);
        Tag tag = mqttManager.main.listOfQTrackTags[rtls.frequency];
        rtlsId.text = "TagID : " + rtls.frequency;
        x_Value.text = "X : " + tag.X.ToString();
        y_Value.text = "Z" + tag.Y.ToString();
        z_Value.text = "altitude deactivated";
    }

}
