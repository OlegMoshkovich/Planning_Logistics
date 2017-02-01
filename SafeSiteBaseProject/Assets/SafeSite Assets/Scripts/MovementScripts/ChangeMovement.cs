using UnityEngine;
using System.Collections;

public class ChangeMovement : MonoBehaviour {
    public GameObject target;

    public void ChangeMovementTo(int movementIndex)
    {
        MovementType movement = (MovementType)movementIndex;
        ChangeMovementTo(movement);
    }

    public void ChangeMovementTo(MovementType movement)
    {
        if (target == null) target = this.gameObject;

        target.GetComponent<SyncedAsset>().movement = movement;

        removeMovementScripts();

        switch (movement)
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
