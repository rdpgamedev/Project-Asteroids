﻿using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameObject FIELDOBJ;
    public GameObject SCOREPARTICLES;
    public GameObject TIMEPARTICLES;
    public GameObject cameraObj;
    public float STARTTIME = 9.99f;
    public bool isPlaying;
    public int level = 1;
    public int score = 0;
    public float time;
    public float difficulty = 0f;
    public Highscores highscores;

    GameObject timeObj;
    PlayerShip ship;
    GameObject field;
    FieldSegment activeSegment;
    bool gameOver = false;

    void Awake ()
    {
        instance = this;
    }
	void Start ()
    {
        timeObj = TIMEPARTICLES.transform.parent.gameObject;
        ship = PlayerShip.instance;
        Startup();
        string highscoresPath = Path.Combine(Application.persistentDataPath, "highscores.xml");
        if (!File.Exists(highscoresPath))
        {
            FileStream stream = File.Create(highscoresPath);
            stream.Close();
            File.WriteAllText(highscoresPath, File.ReadAllText(Path.Combine(Application.dataPath, "defaulthighscores.xml")));
        }
        highscores = Highscores.Load(highscoresPath);
    }
	
	void Update ()
    {
        if (isPlaying)
        {
            time -= Time.deltaTime;
            if (time < 0f)
            {
                time = 0f;
                timeObj.GetComponent<AudioSource>().Stop();
            }
            else if (time < 10f)
            {
                if (!timeObj.GetComponent<AudioSource>().isPlaying) timeObj.GetComponent<AudioSource>().Play();
            }
            TIMEPARTICLES.transform.parent.GetComponent<Animator>().SetFloat("time", time);
        }
	}

    public void Startup ()
    {
        level = 1;
        score = 0;
        time = STARTTIME;
        difficulty = 0f;
        field = GameObject.Instantiate<GameObject>(FIELDOBJ);
    }

    public void ResetTime ()
    {
        time = STARTTIME;
        timeObj.GetComponent<Animator>().SetTrigger("green");
        timeObj.GetComponent<AudioSource>().Stop();
    }

    public void SetActiveSegment (FieldSegment seg)
    {
        if (activeSegment != null) Destroy(activeSegment.gameObject);
        activeSegment = seg;
        activeSegment.SetActive(true);
    }

    public FieldSegment GetActiveSegment ()
    {
        return activeSegment;
    }

    public void Play ()
    {
        isPlaying = true;
        UIManager.instance.ActivateUI(UIManager.UIType.GAME);
        BGM.instance.Activate();
        cameraObj.GetComponent<MoveTo>().enabled = true;
        cameraObj.GetComponent<LookAt>().enabled = true;
        cameraObj.transform.position = ship.transform.FindChild("DefaultCameraPos").position;
        ship.GetComponent<AudioSource>().enabled = true;
    }

    public void GameOver()
    {
        if (!gameOver) SceneManager.LoadSceneAsync("MainScene").allowSceneActivation = true;
        gameOver = true;
    }

    public void Restart()
    {
        Destroy(field);
        ship.Restart();
        cameraObj.GetComponent<CameraManager>().Restart();
        Startup();
        Play();
    }

    public void MenuScreen()
    {
        UIManager.instance.ActivateUI(UIManager.UIType.MENU);
    }

    public void ScoresScreen()
    {
        UIManager.instance.ActivateUI(UIManager.UIType.SCORES);
    }

    public void CreditsScreen()
    {
        UIManager.instance.ActivateUI(UIManager.UIType.CREDITS);
    }

    public void IncreaseScore ()
    {
        ++score;
        if (score % 10 == 0) SCOREPARTICLES.GetComponent<ParticleSystem>().Play();
        SCOREPARTICLES.transform.parent.GetComponent<Animator>().SetTrigger("flash");
        SCOREPARTICLES.transform.parent.GetComponent<AudioSource>().Play();
    }

    public void AddScore(string name)
    {
        Score newScore = new Score(name, score);
        highscores.Push(newScore);
        Debug.Log("Added score by " + name);
    }

    public int GetHighscore ()
    {
        return highscores.TopScore();
    }

    public int GetBottomScore ()
    {
        return highscores.BottomScore();
    }
}
