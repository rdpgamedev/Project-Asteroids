using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour {
    public static Boost instance;

    public GameObject leftThruster;
    public GameObject rightThruster;
    public GameObject camera;
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
        //increase camera shake
        oldCameraShake = camera.GetComponent<Shake>().scale;
        camera.GetComponent<Shake>().scale = boostCameraShake;
        //change flame color
        PlayerShip.instance.GetComponent<Animator>().SetBool("thrusterBoosting", true);
        //activate particles
    }

    void EndBoosting ()
    {
        boosting = false;
        //reset speed
        leftControl.boostScale = 1f;
        rightControl.boostScale = 1f;
        //reset camera shake
        camera.GetComponent<Shake>().scale = oldCameraShake;
        //reset flame color
        PlayerShip.instance.GetComponent<Animator>().SetBool("thrusterBoosting", false);
        //reset particles
    }
}
