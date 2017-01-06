using UnityEngine;
using System.Collections;
 
public class CameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;
 
 	void Start()
 	{
 		m_Camera = Camera.main;
 	}
    void Update()
    {
    	 transform.LookAt(m_Camera.transform.position);
        //transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back,
          //  m_Camera.transform.rotation * Vector3.up);
    }
}