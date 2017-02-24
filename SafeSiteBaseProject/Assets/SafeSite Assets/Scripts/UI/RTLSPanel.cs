using UnityEngine;
using UnityEngine.UI;

public class RTLSPanel : MonoBehaviour {
    public RTLSMovement rtls;

    public Text rtlsId;
    public Text x_Value;
    public Text y_Value;
    public Text z_Value;

    

    private void Update()
    {
        Tag tag = mqttManager.main.listOfQTrackTags[rtls.frequency];
        if(rtlsId != null) rtlsId.text = "TagID : " + rtls.frequency;
        if(x_Value != null) x_Value.text = "X : " + tag.X.ToString();
        if( y_Value != null) y_Value.text = "Z" + tag.Y.ToString();
        if( z_Value != null) z_Value.text = "altitude deactivated";
    }

}
