using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
    public GameObject MULTIPLIERTEXT;
    public GameObject SCORETEXT;
    public GameObject TIMETEXT;
    public GameObject VELOCITYTEXT;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateMultiplier();
        UpdateScore();
        UpdateTime();
        UpdateVel();
	}

    void UpdateMultiplier ()
    {
        MULTIPLIERTEXT.GetComponent<TextMesh>().text =
            "x " + GameManager.instance.multiplier.ToString("N2");
    }

    void UpdateScore ()
    {
        SCORETEXT.GetComponent<TextMesh>().text =
            "SCORE: " + GameManager.instance.score;
    }

    void UpdateTime ()
    {
        TIMETEXT.GetComponent<TextMesh>().text = 
            GameManager.instance.time.ToString("00.00");
    }

    void UpdateVel ()
    {
        VELOCITYTEXT.GetComponent<TextMesh>().text = 
            (int)PlayerShip.instance.GetComponent<Rigidbody>().velocity.magnitude + " m/s";
    }
}
