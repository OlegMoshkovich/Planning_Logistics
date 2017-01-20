using UnityEngine;
using System.Collections;

public class FindCollisions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    if(GetComponent<BoxCollider>() == null & GetComponent<MeshCollider>() == null & GetComponent<CapsuleCollider>() == null )
        {
            MeshCollider addedCollider = this.gameObject.AddComponent<MeshCollider>();
            addedCollider.isTrigger = true;
        }
	}

    private void OnTriggerEnter(Collider collider)
    {
        HazardManager.main.AddCollision(collider);
    }

}
