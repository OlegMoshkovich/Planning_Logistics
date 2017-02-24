using UnityEngine;

public class FindCollisions : MonoBehaviour {

	protected void Start () {
        Collider collider = AssetManager.CheckCollider(gameObject);
        collider.isTrigger = true;
	}

    protected void OnTriggerEnter(Collider collider)
    {
        HazardManager.main.AddCollision(collider);
    }

}
