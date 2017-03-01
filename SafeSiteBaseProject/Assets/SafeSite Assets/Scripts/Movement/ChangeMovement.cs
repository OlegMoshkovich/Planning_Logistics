using UnityEngine;
using System.Collections;

public class ChangeMovement : MonoBehaviour {
    public GameObject target;
    public MovementType movement;

    public void ChangeMovementTo(int movementIndex)
    {
        MovementType movement = (MovementType)movementIndex;
        ChangeMovementTo(movement);
    }
    public void ChangeMovementTo()
    {
        if (movement != null) ChangeMovementTo(movement);
        else Debug.Log("movement must be set");
    }
    public void OnEnable()
    {
        if (AssetPanel.main != null) target = AssetPanel.main.selectedAsset;
        else Debug.LogError("Missing AssetPanel");
    }

    public void ChangeMovementTo(MovementType setMovement)
    {
        if (target == null) target = this.gameObject;

        target.GetComponent<SyncedAsset>().sa_movement = setMovement;

        removeMovementScripts();

        switch (setMovement)
        {
            case MovementType.Random:
                target.AddComponent<RandomMovement>();
                break;
            case MovementType.SetMovement:
                target.AddComponent<SetPointsMovement>();
                break;
            case MovementType.RTLS:
                target.AddComponent<RTLSMovement>();
                break;
            case MovementType.Static:
                if(target.GetComponent<NavMeshAgent>()!= null) target.GetComponent<NavMeshAgent>().destination = target.transform.position;
                if(target.GetComponent<Animator>() != null) target.GetComponent<Animator>().SetFloat("Speed", 0);
                break;

        }
    }
    
    public void removeMovementScripts()
    {
        if (target.GetComponent<RandomMovement>() != null) Destroy(target.GetComponent<RandomMovement>());
        if (target.GetComponent<SetPointsMovement>() != null) Destroy(target.GetComponent<SetPointsMovement>());
        if (target.GetComponent<RTLSMovement>() != null) Destroy(target.GetComponent<RTLSMovement>());
    }
}
