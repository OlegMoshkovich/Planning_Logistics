using UnityEngine;
using System.Collections;

public class FeedbackButton : MonoBehaviour {

    public void OnClick_Handler()
    {
        Application.OpenURL("http://www.hcsafety.com/support/");
    }
}
