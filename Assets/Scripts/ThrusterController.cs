using UnityEngine;
using System.Collections;

public class ThrusterController : MonoBehaviour
{
    public static float MAXTHRUST;
    public bool isLeft = false;
    public bool active = false;

    private float TRIM = 0.6f; //pitch trim
    private float maxXRotation = 18f;
    private float maxYRotation = 16f;
    private float thrust = 4f/6f; //initial thrust before activation
    private float xaxis;
    private float yaxis;
    private float thrustScale;

    void Start()
    {
        MAXTHRUST = PlayerShip.instance.MAXTHRUST;
        transform.localEulerAngles = new Vector3(TRIM, 0, 0);
    }

    void FixedUpdate()
    {
        thrustScale = PlayerShip.instance.thrustScale;
        if (active) RotateThruster();

        //apply thrust
        PlayerShip ship = PlayerShip.instance;
        Rigidbody shipbody = ship.GetComponent<Rigidbody>();
        Vector3 direction = transform.forward;
        Vector3 force = direction * thrust * thrustScale;
        if (!GameManager.instance.isPlaying) force *= 0f;
        //use point between thruster and ship as application point
        Vector3 thrusterpos = this.transform.position;
        Vector3 offset = ship.transform.position - thrusterpos;
        float offsetMin = PlayerShip.instance.THRUSTER_OFFSET_MIN;
        if (offsetMin > 0.5f) offsetMin = 0.5f;
        if (offsetMin < 0f) offsetMin = 0f;
        offset *= thrust / 2 + offsetMin;
        Vector3 forcepos = thrusterpos + offset;
        shipbody.AddForceAtPosition(force, forcepos);

        ParticleSystem thrusterParticles = 
            transform.FindChild("Thruster_Particles").GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emission = thrusterParticles.emission;
        emission.rateOverTime = 1000f * (thrust - (13f / 24f)) * 24f / 11f + 50f;
    }

    public void ResetRotation ()
    {
        transform.localEulerAngles = new Vector3(TRIM, 0, 0);
        thrust = 4f / 6f;
    }

    void RotateThruster()
    {
        if (isLeft)
        {
            xaxis = PlayerController.yaxisleft;
            yaxis = PlayerController.xaxisleft;
            thrust = ThrustCalculation(yaxis, xaxis);
        }
        else
        {
            xaxis = PlayerController.yaxisright;
            yaxis = PlayerController.xaxisright;
            thrust = ThrustCalculation(-yaxis, xaxis);
        }
        float xrot = -xaxis * maxXRotation + TRIM;
        float yrot = -yaxis * maxYRotation;
        transform.localEulerAngles = new Vector3(xrot, yrot, 0);
    }
    
    //calculates thrust based on yaxis and xaxis of controller
    //yaxis is positive in direction toward center of controller
    // (e.g. when the left stick is pushed to the right)
    float ThrustCalculation (float yaxis, float xaxis)
    {
        return (yaxis + 1f) / 6f + (2f / 3f) - Mathf.Abs(xaxis) / 8f;
    }
}
