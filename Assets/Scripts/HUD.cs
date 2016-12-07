using UnityEngine;
using UnityEngine.UI;
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
        MULTIPLIERTEXT.GetComponent<Text>().text =
            "x " + GameManager.instance.multiplier.ToString("N0");
    }

    void UpdateScore ()
    {
        int score = GameManager.instance.score;
        int thousands = 0;
        while (score > 999)
        {
            score /= 1000;
            ++thousands;
        }
        string padding = "";
        if (score < 10) padding += " ";
        if (score < 100) padding += " ";
        string suffix = "";
        switch (thousands)
        {
            case 0:
                suffix = " ";
                break;
            case 1:
                suffix = "K";
                break;
            case 2:
                suffix = "M";
                break;
            case 3:
                suffix = "B";
                break;
            case 4:
                suffix = "T";
                break;
            case 5:
                suffix = "Q";
                break;
            default:
                suffix = "?!?";
                break;
        }
        SCORETEXT.GetComponent<Text>().text =
            "SCORE: " + padding + score + suffix;
    }

    void UpdateTime ()
    {
        TIMETEXT.GetComponent<Text>().text = 
            GameManager.instance.time.ToString("00.00");
    }

    void UpdateVel ()
    {
        VELOCITYTEXT.GetComponent<TextMesh>().text = 
            (int)PlayerShip.instance.GetComponent<Rigidbody>().velocity.magnitude + " m/s";
    }
}
