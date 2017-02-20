using UnityEngine;
using System.Collections;

public class UserSettings : MonoBehaviour
{

    public static UserSettings main;
    public string userName = "Mr. User";


    void Start()
    {
        main = this;
    }

}
