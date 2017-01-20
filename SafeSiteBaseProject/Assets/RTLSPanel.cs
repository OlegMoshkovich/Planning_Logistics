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
        Tag tag = mqttManager.main.listOfQTrackTags[rtls.frequency];
        rtlsId.text = rtls.frequency;
        x_Value.text = tag.X.ToString();
        y_Value.text = tag.Y.ToString();
        z_Value.text = "deactivated";
    }

}
