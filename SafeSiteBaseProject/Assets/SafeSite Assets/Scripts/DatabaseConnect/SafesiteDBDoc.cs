using UnityEngine;
using System.Collections;


[System.Serializable]
public class SafesiteDBDoc {
    public SyncedAsset asset;
    public SyncedHazard hazard;
    public RTLSMovement rtls;

    public static SafesiteDBDoc CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SafesiteDBDoc>(jsonString);

    }
}
