using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public float STARTTIME = 9.99f;
    public int level = 1;
    public int score = 0;
    public float time;
    public float difficulty = 0f;

    PlayerShip ship;
    FieldSegment activeSegment;
    bool gameOver = false;

    void Awake ()
    {
        instance = this;
    }
	void Start ()
    {
        ship = PlayerShip.instance;
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

    public void GameOver()
    {
        if (!gameOver) SceneManager.LoadSceneAsync("MainScene").allowSceneActivation = true;
        gameOver = true;
    }
}
