using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour {
    public static Boost instance;

    public GameObject leftThruster;
    public GameObject rightThruster;
    public GameObject cameraObj;
    public GameObject boostParticles;
    public float boostScale = 1.2f;
    public float boostCameraShake = 1f;

    private bool boosting;
    private ThrusterController leftControl;
    private ThrusterController rightControl;
    private float oldCameraShake;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        leftControl = leftThruster.GetComponent<ThrusterController>();
        rightControl = rightThruster.GetComponent<ThrusterController>();
    }

    void Update () {
        if (ThrustersBoosting() && !boosting)
        {
            StartBoosting();
        }
        else if (!ThrustersBoosting() && boosting)
        {
            EndBoosting();
        }
    }

    bool ThrustersBoosting ()
    {
        return (leftControl.isBoosting() && rightControl.isBoosting());
    }

    void StartBoosting ()
    {
        boosting = true;
        //increase speed
        leftControl.boostScale = boostScale;
        rightControl.boostScale = boostScale;
        leftControl.ConstrainRotation();
        rightControl.ConstrainRotation();
        //increase camera shake
        oldCameraShake = cameraObj.GetComponent<Shake>().scale;
        cameraObj.GetComponent<Shake>().scale = boostCameraShake;
        //change flame color
        PlayerShip.instance.GetComponent<Animator>().SetBool("thrusterBoosting", true);
        //activate particles
        boostParticles.GetComponent<ParticleSystem>().Play();
    }

    void EndBoosting ()
    {
        boosting = false;
        //reset speed
        leftControl.boostScale = 1f;
        rightControl.boostScale = 1f;
        leftControl.ReleaseRotation();
        rightControl.ReleaseRotation();
        //reset camera shake
        cameraObj.GetComponent<Shake>().scale = oldCameraShake;
        //reset flame color
        PlayerShip.instance.GetComponent<Animator>().SetBool("thrusterBoosting", false);
        //reset particles
        boostParticles.GetComponent<ParticleSystem>().Stop();
    }
}
