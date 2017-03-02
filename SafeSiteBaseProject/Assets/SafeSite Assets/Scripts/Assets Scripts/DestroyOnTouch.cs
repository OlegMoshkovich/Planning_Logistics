using UnityEngine;
using System.Collections;

public class DestroyOnTouch : MonoBehaviour {
    protected void Awake()
    {
        AssetManager.CheckCollider(this.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collided with " + collision.collider.gameObject);
        AssetManager.main.DeleteAsset(collision.collider.gameObject);
    }
}
