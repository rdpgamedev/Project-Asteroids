using UnityEngine;
using System.Collections;

public class ThrusterController : MonoBehaviour
{
    public static float MAXTHRUST = 10f;
    public bool isLeft = false;
    public float maxRotation = 30f;

    float thrust = 0.3f;

    // Use this for initialization
    void Start()
    {
        MAXTHRUST = PlayerShip.instance.MAXTHRUST;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xaxis;
        float yaxis;
        if (isLeft)
        {
            xaxis = PlayerController.yaxisleft;
            yaxis = -PlayerController.xaxisleft;
            thrust = (-yaxis + 1f) / 4 + 0.5f;
        }
        else
        {
            xaxis = PlayerController.yaxisright;
            yaxis = -PlayerController.xaxisright;
            thrust = ((yaxis + 1f) / 4) + 0.5f;
        }
        float xrot = -xaxis * maxRotation;
        float yrot = yaxis * maxRotation / 2;
        transform.localEulerAngles = new Vector3(xrot, yrot, 0);

        //apply thrust
        PlayerShip ship = PlayerShip.instance;
        Rigidbody shipbody = ship.GetComponent<Rigidbody>();
        Vector3 direction = transform.forward;
        Vector3 force = direction * thrust * thrust * MAXTHRUST;
        //use point between thruster and ship as application point
        Vector3 thrusterpos = this.transform.position;
        Vector3 offset = ship.transform.position - thrusterpos;
        offset *= thrust / 2 + 0.25f;
        Vector3 forcepos = thrusterpos + offset;
        shipbody.AddForceAtPosition(force, forcepos);
    }
}
