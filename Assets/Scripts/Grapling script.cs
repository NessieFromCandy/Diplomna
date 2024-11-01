using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graplingscript : MonoBehaviour
{
    private LineRenderer Ir;
    private Vector3 grapplePoint;
    public LayerMask WhatisGrapplePoint;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;



    void Awake()
    {
        Ir = GetComponent<LineRenderer>();

    }



    void Update()
    {
       

        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }


    }



    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(origin: camera.position, direction: camera.forward, out hit, maxDistance, WhatisGrapplePoint))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;


            float distanceFromPoint = Vector3.Distance(a: player.position, b: grapplePoint);


            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;


            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            Ir.positionCount = 2;
        }


    }



    private void LateUpdate()
    {
        DrawRope(); 
    }

    void DrawRope()
    {
        if (!joint) return;
         
        Ir.SetPosition(index: 0, gunTip.position);
        Ir.SetPosition(index: 1, grapplePoint);

    }


    void StopGrapple()
    {
        Ir.positionCount = 0;
        Destroy(joint);

    }




}

