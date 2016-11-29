using UnityEngine;
using System.Collections;

[System.Serializable]
public class HCSTag {
    public string macAddress;
    public float[] ori;
    public float[] gyr;
    public float light;
    public float humidity;
    public float uvi;
    public float temperature;
    public float altitude;

    public static HCSTag CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<HCSTag>(jsonString);
    }
}
