using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public float STARTTIME = 9.99f;
    public int level = 1;
    public int score = 0;
    public float multiplier = 1.0f;
    public float time;
    public float difficulty = 0f;

    FieldSegment activeSegment;

    void Awake ()
    {
        instance = this;
    }
	void Start ()
    {
        Startup();
	}
	
	void Update ()
    {
        time -= Time.deltaTime;
	}

    public void Startup ()
    {
        level = 1;
        score = 0;
        multiplier = 1.0f;
        time = STARTTIME;
        difficulty = 0f;
    }

    public void ResetTime ()
    {
        time = STARTTIME;
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
