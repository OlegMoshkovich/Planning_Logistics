using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class RTLSMovement : MonoBehaviour {
    public string frequency;
    public int rtlsPrecision = 5;
 
    private GameObject uncertaintyArea;


    // Use this for initialization
    void createUncertaintyCircle()
    {
        uncertaintyArea = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(uncertaintyArea.GetComponent<CapsuleCollider>());
        uncertaintyArea.GetComponent<Renderer>().material = Resources.Load<Material>("RTLSAreaMaterial");
        uncertaintyArea.transform.localScale = new Vector3(rtlsPrecision, 0.01f, rtlsPrecision);
        uncertaintyArea.transform.parent = this.gameObject.transform;
        uncertaintyArea.transform.localPosition = new Vector3(0, 0.2f, 0);
        MeshCollider collider = uncertaintyArea.AddComponent<MeshCollider>();
        collider.convex = true;
        collider.isTrigger = true;
    }
    void Start()
    {
        createUncertaintyCircle();
    }
    private void OnDestroy()
    {
        Destroy(uncertaintyArea);
    }
    private void OnEnable()
    {
        if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed = 5;
    }
    void OnDisable()
    {
        if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed = 3;
    }
    // Update is called once per frame
    void Update () {
        //Update Animator speed
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetFloat("Speed", GetComponent<NavMeshAgent>().speed);
        }

        try
        {
            Tag tag = mqttManager.main.listOfQTrackTags[frequency];
            GetComponent<NavMeshAgent>().destination = new Vector3(tag.X, 0.1f, tag.Y);
            GetComponent<NavMeshAgent>().speed = 10;
            //transform.Translate(tag.X, 0, tag.Y, Space.World);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
  
    }
}
