using UnityEngine;
using System.Collections;


//Allows User to click and Drag GameObject
public class ClickAndDrag : MonoBehaviour {

        private Vector3 screenPoint;
        
        private void Start()
        {
            var collider = AssetManager.CheckCollider(this.gameObject);
            collider.isTrigger = true;
        }
        void OnMouseDown()
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        }
        void OnMouseDrag()
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            transform.position = Camera.main.ScreenToWorldPoint(curScreenPoint);
        }
    }
