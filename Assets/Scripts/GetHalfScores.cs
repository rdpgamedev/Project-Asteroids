using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHalfScores: MonoBehaviour {
    public bool firstHalf;

    private Text text;

	void Start () {
        text = GetComponent<Text>();
        Reload();
	}
	
	void Update () {
		
	}

    public void Reload ()
    {
        Highscores highscores = GameManager.instance.highscores;
        int index = 5;
        if (firstHalf) index = 0;
        string scores = "";
        for (int i = index; i < index + 5 && i < highscores.Scores.Count; ++i)
        {
            Score score = highscores.Scores[i];
            scores += score.name + " - " + score.score + '\n';
        }
        text.text = scores;
    }
}
