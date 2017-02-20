using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    Text text;
	void Start () {
        text = GetComponent<Text>();
	}
	
	void Update () {
        UpdateScore();
	}

    void UpdateScore()
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
        text.text =
            "SCORE: " + padding + score + suffix;
    }
}
