using UnityEngine;

[System.Serializable]
public class Tag
{
    public float X;
    public float Y;
    public string Frequency;

    public static Tag CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Tag>(jsonString);
    }

}