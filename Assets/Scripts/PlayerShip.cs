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
    public bool isDead = false;

    private float oldFov;
    private Vector3 firstPosition;
    private Quaternion firstRotation;
    private float firstThrustScale;
    private GameManager gameManager;

    void Awake ()
    {
        instance = this;
        firstPosition = transform.position;
        firstRotation = transform.rotation;
        firstThrustScale = thrustScale;
    }

	void Start ()
    {
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        gameManager = GameManager.instance;
	}
	
	void Update ()
    {
        if (thrustScale > MAXTHRUST) thrustScale = MAXTHRUST;
        if (GameManager.instance.time == 0f && !isDead) StartDeath();
        if (isDead && explosionParticles.GetComponent<ParticleSystem>().isStopped && GameManager.instance.isPlaying)
        {
            GameManager.instance.isPlaying = false;
            Time.timeScale = 0f;
            if (gameManager.score < gameManager.GetBottomScore())
            {
                GameManager.instance.Pause();
            }
            else
            {
                GameManager.instance.Pause();
                UIManager.instance.ActivateUI(UIManager.UIType.HIGHSCORE);
            }
            
            
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
        transform.position = new Vector3(transform.position.x, transform.position.y, -250f);
        transform.rotation = firstRotation;
        thrustScale = firstThrustScale;
        isDead = false;
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
        if (GameManager.instance.isPlaying)
        {
            if (!isDead) StartDeath();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }

    void StartDeath ()
    {
        isDead = true;
        explosionParticles.GetComponent<ParticleSystem>().Play();
        explosionParticles.GetComponent<AudioSource>().Play();
        Invoke("HideVisualParts", 0.1f);
        cameraObj.GetComponent<LookAt>().enabled = false;
        cameraObj.GetComponent<MoveTo>().enabled = false;
        cameraObj.GetComponent<Shake>().enabled = false;
        BGM.instance.SetVolume(0.3f);
    }

    void HideVisualParts ()
    {
        shipModel.SetActive(false);
        leftthruster.SetActive(false);
        rightthruster.SetActive(false);
        transform.FindChild("VelocityChevrons").gameObject.SetActive(false);
        GetComponent<AudioSource>().enabled = false;
    }
    
    void ShowVisualParts ()
    {
        shipModel.SetActive(true);
        leftthruster.SetActive(true);
        rightthruster.SetActive(true);
        transform.FindChild("VelocityChevrons").gameObject.SetActive(true);
    }
}
