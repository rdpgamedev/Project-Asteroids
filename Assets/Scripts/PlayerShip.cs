/*
 * Handles the state of the player ship.
 * This includes movement, combat, pickups, etc.
 */

using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour
{

    //instance of PlayerShip for other scripts to access
    public static PlayerShip instance;
    public GameObject leftthruster;
    public GameObject rightthruster;
    public GameObject cameraObj;
    public float MAXTHRUST = 60f;
    public float thrustScale = 20f;
    public float thrusterOffsetMin = 0.35f;
    public float fovScale;
    public float fovMinVel;
    public float fovRange;

    void Awake ()
    {
        instance = this;
    }

	void Start ()
    {
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	void Update ()
    {
        if (thrustScale > MAXTHRUST) thrustScale = MAXTHRUST;
        
	}

    public void ActivateThrusters()
    {
        leftthruster.GetComponent<ThrusterController>().active = true;
        rightthruster.GetComponent<ThrusterController>().active = true;
    }

    private void LateUpdate()
    {
        float originalfov = cameraObj.GetComponent<Camera>().fieldOfView;
        float velocity = GetComponent<Rigidbody>().velocity.magnitude;
        cameraObj.GetComponent<Camera>().fieldOfView *= Mathf.Sqrt(velocity / fovMinVel) * fovScale;
        if (cameraObj.GetComponent<Camera>().fieldOfView >= originalfov + fovRange)
        {
            cameraObj.GetComponent<Camera>().fieldOfView = originalfov + fovRange;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }
}
