using UnityEngine;
using System.Collections;

public class ClickAndDrag : MonoBehaviour {

        private Vector3 screenPoint;
        private Vector3 offset;
        
        
        private void Start()
        {
            //Check Object has collider, if not add one
            if (GetComponent<MeshCollider>() == null & GetComponent<BoxCollider>() == null)
            {
                MeshCollider collider = this.gameObject.AddComponent<MeshCollider>();
                collider.isTrigger = true;
            }
        }

        void OnMouseDown()
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        }
        void OnMouseDrag()
        {
            float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));

        }
    }
