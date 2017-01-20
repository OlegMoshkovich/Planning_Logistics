using UnityEngine;
using System.Collections;

public class ClickAndDrag : MonoBehaviour {

        private Vector3 screenPoint;
        
        
        private void Start()
        {
            //Check Object has collider, if not add one
            if (GetComponent<MeshCollider>() == null & GetComponent<BoxCollider>() == null & GetComponent<CapsuleCollider>())
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
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            transform.position = curPosition;

    }
    }
