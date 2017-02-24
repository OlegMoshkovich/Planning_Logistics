using UnityEngine;
using AppAdvisory.VSGIF;

public class reportButton : MonoBehaviour {
    private bool clicked = false;

    private void Start()
    {
        Record.OnSavedGIFEvent += onSavedGifEventHandler;
    }

    private void onSavedGifEventHandler(SaveState savestate)
    {
        if(savestate == SaveState.Done)
        {
  
        }
    }


    public void onClickReportButtonHandler()
    {
        Record.DORec();
        if (clicked)
        {
            Record.DOSave();        
        } 
        clicked = !clicked;
    }

}
