/*
 * Handles the state of the player ship.
 * This includes movement, combat, pickups, etc.
 */

using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour
{
    public static PlayerShip instance; //instance of PlayerShip for other scripts to access
    public GameObject leftthruster;
    public GameObject rightthruster;
    public GameObject cameraObj;
    public GameObject explosionParticles;
    public GameObject shipModel;
    public GameObject menuUI;
    public float MAXTHRUST;
    public float THRUSTER_OFFSET_MIN;
    public float thrustScale;
    public float fovScale;
    public float fovMinVel;
    public float fovRange;

    private bool isDead = false;
    private float oldFov;
    private Vector3 firstPosition;
    private Quaternion firstRotation;
    private float firstThrustScale;

    void Awake ()
    {
        instance = this;
    }

	void Start ()
    {
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        firstPosition = transform.position;
        firstRotation = transform.rotation;
        firstThrustScale = thrustScale;
	}
	
	void Update ()
    {
        if (thrustScale > MAXTHRUST) thrustScale = MAXTHRUST;
        if (GameManager.instance.time == 0f && !isDead) StartDeath();
        if (isDead && explosionParticles.GetComponent<ParticleSystem>().isStopped)
        {
            UIManager.instance.ActivateUI(UIManager.UIType.PAUSE);
        }
	}

    public void ActivateThrusters ()
    {
        leftthruster.GetComponent<ThrusterController>().active = true;
        rightthruster.GetComponent<ThrusterController>().active = true;
    }

    public void DeactivateThrusters ()
    {
        leftthruster.GetComponent<ThrusterController>().active = false;
        rightthruster.GetComponent<ThrusterController>().active = false;
    }

    public void ResetThrusters ()
    {
        leftthruster.GetComponent<ThrusterController>().ResetRotation();
        rightthruster.GetComponent<ThrusterController>().ResetRotation();
    }

    public void Restart ()
    {
        ShowVisualParts();
        DeactivateThrusters();
        ResetThrusters();
        GetComponent<Rigidbody>().velocity = new Vector3();
        Quaternion zeroRotation = new Quaternion();
        zeroRotation.eulerAngles = new Vector3();
        GetComponent<Rigidbody>().rotation = zeroRotation;
        GetComponent<Rigidbody>().freezeRotation = true;
        transform.position = firstPosition;
        transform.rotation = firstRotation;
        thrustScale = firstThrustScale;
    }

    private void LateUpdate ()
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

    void OnCollisionEnter (Collision collision)
    {
        if (!isDead) StartDeath();
    }

    void StartDeath ()
    {
        isDead = true;
        explosionParticles.GetComponent<ParticleSystem>().Play();
        Invoke("HideVisualParts", 0.1f);
        cameraObj.GetComponent<LookAt>().enabled = false;
        cameraObj.GetComponent<MoveTo>().enabled = false;
    }

    void HideVisualParts ()
    {
        shipModel.SetActive(false);
        leftthruster.SetActive(false);
        rightthruster.SetActive(false);
        transform.FindChild("VelocityChevrons").gameObject.SetActive(false);
    }
    
    void ShowVisualParts ()
    {
        shipModel.SetActive(true);
        leftthruster.SetActive(true);
        rightthruster.SetActive(true);
        transform.FindChild("VelocityChevrons").gameObject.SetActive(true);
    }
}
