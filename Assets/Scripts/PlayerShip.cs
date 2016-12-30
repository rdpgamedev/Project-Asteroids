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
    public GameObject explosionParticles;
    public GameObject shipModel;
    public GameObject menuUI;
    public float MAXTHRUST = 60f;
    public float thrustScale = 20f;
    public float thrusterOffsetMin = 0.35f;
    public float fovScale;
    public float fovMinVel;
    public float fovRange;

    private bool isDead = false;
    private float oldFov;

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
        if (isDead && explosionParticles.GetComponent<ParticleSystem>().isStopped)
        {
            UIManager.instance.ActivateUI(UIManager.UIType.MENU);
        }
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
        if (!isDead)
        {
            cameraObj.GetComponent<Camera>().fieldOfView *= Mathf.Sqrt(velocity / fovMinVel) * fovScale;
            oldFov = cameraObj.GetComponent<Camera>().fieldOfView;
        }else
        {
            cameraObj.GetComponent<Camera>().fieldOfView = oldFov;
        }
        if (cameraObj.GetComponent<Camera>().fieldOfView > originalfov + fovRange)
        {
            cameraObj.GetComponent<Camera>().fieldOfView = originalfov + fovRange;
        }else if (cameraObj.GetComponent<Camera>().fieldOfView < originalfov)
        {
           cameraObj.GetComponent<Camera>().fieldOfView = originalfov;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDead) StartDeath();
    }

    void StartDeath()
    {
        isDead = true;
        explosionParticles.GetComponent<ParticleSystem>().Play();
        DestroyVisualParts(0.1f);
        cameraObj.GetComponent<LookAt>().enabled = false;
        cameraObj.GetComponent<MoveTo>().enabled = false;
    }

    void DestroyVisualParts(float time)
    {
        Destroy(shipModel, time);
        Destroy(leftthruster, time);
        Destroy(rightthruster, time);
        Destroy(transform.FindChild("VelocityChevrons").gameObject);
    }
}
