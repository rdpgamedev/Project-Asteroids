using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public float STARTTIME;

    int level;
    int score;
    float time;
    float difficulty;
    FieldSegment activeSegment;
    void Awake ()
    {
        instance = this;
    }
	void Start ()
    {
	}
	
	void Update ()
    {
        time -= Time.deltaTime;
	}

    public void SetActiveSegment (FieldSegment seg)
    {
        if (activeSegment != null) Destroy(activeSegment.gameObject); ;
        activeSegment = seg;
        activeSegment.SetActive(true);
    }

    public FieldSegment GetActiveSegment ()
    {
        return activeSegment;
    }
}
