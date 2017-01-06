using UnityEngine;

[System.Serializable]
public class Tag
{
    public float X;
    public float Y;
    public int Frequency;

    public static Tag CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Tag>(jsonString);
    }

    // Given JSON input:
    // {"X":"345","Y":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.

}