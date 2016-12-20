﻿using UnityEngine;
using System.Collections;

public class ThrusterController : MonoBehaviour
{
    public static float MAXTHRUST;
    public bool isLeft = false;

    private float maxXRotation = 18f;
    private float maxYRotation = 16f;
    private float thrust = 0.3f;
    private float xaxis;
    private float yaxis;
    private float thrustScale;

    // Use this for initialization
    void Start()
    {
        MAXTHRUST = PlayerShip.instance.MAXTHRUST;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        thrustScale = PlayerShip.instance.thrustScale;
        if (isLeft)
        {
            xaxis = PlayerController.yaxisleft;
            yaxis = -PlayerController.xaxisleft;
            thrust = ((-yaxis + 1f) / 6f + (2f/3f) - Mathf.Abs(xaxis)/8f);
        }
        else
        {
            xaxis = PlayerController.yaxisright;
            yaxis = -PlayerController.xaxisright;
            thrust = ((yaxis + 1f) / 6f + (2f/3f) - Mathf.Abs(xaxis)/8f);
        }
        float xrot = -xaxis * maxXRotation;
        float yrot = yaxis * maxYRotation;
        transform.localEulerAngles = new Vector3(xrot, yrot, 0);

        //apply thrust
        PlayerShip ship = PlayerShip.instance;
        Rigidbody shipbody = ship.GetComponent<Rigidbody>();
        Vector3 direction = transform.forward;
        Vector3 force = direction * thrust * thrustScale;
        //use point between thruster and ship as application point
        Vector3 thrusterpos = this.transform.position;
        Vector3 offset = ship.transform.position - thrusterpos;
        float offsetMin = PlayerShip.instance.thrusterOffsetMin;
        if (offsetMin > 0.5f) offsetMin = 0.5f;
        if (offsetMin < 0f) offsetMin = 0f;
        offset *= thrust / 2 + offsetMin;
        Vector3 forcepos = thrusterpos + offset;
        shipbody.AddForceAtPosition(force, forcepos);
    }
}
