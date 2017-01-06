using UnityEngine;
using System.Collections;

[System.Serializable]
public class HCSTag {
    //Example JSON Incoming Received:
    // {"ori":[13.42,-11.86,0.00],"acc":[2.39,-2.16,-10.00],"gyr":[12.22,-1.16,2.76]}
    //  {"lit":0.00,"snd":0.00,"hum":4.70,"bmp":99933.39,"uvi":-0.02,"tmp":29.29,"alt":115.96}
    // {"bat":4.63,"tim":2416.16}

    public string macAddress;
    //Orin
    public float[] ori;
    public float[] gyr;
    public float[] acc;
    //Envi
    public float lit;
    public float hum;
    public float bmp;
    public float uvi;
    public float tmp; // Temperature in Celsius
    public float alt;
    public float snd;
    //Meta
    public float tim;
    public float bat;

    public static HCSTag CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<HCSTag>(jsonString);
    }
}
