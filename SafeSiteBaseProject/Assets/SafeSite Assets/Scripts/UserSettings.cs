using UnityEngine;
using System.Collections;

public class UserSettings : MonoBehaviour
{
    public static UserSettings main;

    //In the future this data will have to be pulled from Online for security purposes
    public string userName = "Mr. User";
    public string objectStorageContainerName;
    public string cloudantDatabaseName;


    void Start()
    {
        main = this;
    }

}
