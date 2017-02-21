using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour {
    public static Boost instance;

    public GameObject leftThruster;
    public GameObject rightThruster;
    public float boostScale = 1.2f;

    private bool boosting;
    private ThrusterController leftControl;
    private ThrusterController rightControl;
    private Color oldFlameColor;
    private Color boostFlameColor;

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
        //reset flame color
        PlayerShip.instance.GetComponent<Animator>().SetBool("thrusterBoosting", false);
        //reset particles
    }
}
