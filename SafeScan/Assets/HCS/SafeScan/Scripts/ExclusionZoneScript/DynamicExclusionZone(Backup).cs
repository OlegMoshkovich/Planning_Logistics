using UnityEngine;
using System.Collections;

namespace Safescan{
	public enum String { cylinder, cube, sphere }
	public class ExclusionZone : MonoBehaviour {
		public String ExclusionShape;
		public Material ExclusionZoneColor;
		public bool changeWithVelocity;
		public bool changeIfObjectChangesSize;
		public bool placeOnGround;
		public Vector3 addSizeFactor = new Vector3(2,0,2);
		public float velocitySizeFactor = 5;
		public float maxVelocity = 5;

		private Vector3 velocity;
		private Vector3 previousPosition;
		private Vector3 initialScale;

		private GameObject exclusionZone;
		// Use this for initialization
		void Start () {

			//Set Shape
			switch (ExclusionShape) {
			case String.sphere:
				exclusionZone = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				Destroy (exclusionZone.GetComponent<SphereCollider> ());
				exclusionZone.AddComponent<MeshCollider> ();
				exclusionZone.GetComponent<MeshCollider> ().isTrigger = true;
					break;
			case String.cylinder:
				exclusionZone = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
				exclusionZone.GetComponent<CapsuleCollider> ().isTrigger = true;
				break;
			case String.cube:
				exclusionZone = GameObject.CreatePrimitive (PrimitiveType.Cube);
				exclusionZone.GetComponent<BoxCollider> ().isTrigger = true;
				break;
			}

			//Set Parent
			exclusionZone.transform.parent = this.transform;
			//Set Color
			exclusionZone.GetComponent<Renderer>().material = ExclusionZoneColor;
			//Set Scale
			exclusionZone.transform.localScale = this.GetComponent<MeshRenderer>().bounds.size;
			exclusionZone.transform.localScale += addSizeFactor;
			initialScale = exclusionZone.transform.localScale;
		}


		void Update () {
			//Update Previous Position
			previousPosition = transform.position;

			//Set Position
			if (placeOnGround) {
				exclusionZone.transform.localPosition = new Vector3 (0, -this.GetComponent<Renderer> ().bounds.extents.y, 0);
			}
			if (!placeOnGround)
				exclusionZone.transform.localPosition = new Vector3 (0, 0, 0);

			//Update with object size change
			if (changeIfObjectChangesSize) {
				exclusionZone.transform.localScale = this.GetComponent<MeshRenderer>().bounds.size;
				exclusionZone.transform.localScale += addSizeFactor;
			}
			//Update Exclusion Zone Scale with Velocity
			if (changeWithVelocity && velocity !=Vector3.zero) {
				exclusionZone.transform.localScale = initialScale*Mathf.InverseLerp (0, maxVelocity, velocity.magnitude) * velocitySizeFactor; 
				exclusionZone.transform.rotation = Quaternion.LookRotation (velocity);
				exclusionZone.transform.Translate (Vector3.forward * Mathf.InverseLerp(0, maxVelocity, velocity.magnitude)* exclusionZone.GetComponent<Renderer>().bounds.extents.magnitude/-2);
				Debug.Log (velocity);
				Debug.Log (Mathf.InverseLerp(0, maxVelocity, velocity.magnitude));
			}
		}

		void LateUpdate(){
			velocity = (previousPosition - transform.position) / Time.deltaTime;
		}
	}
}
