using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameObject FIELDOBJ;
    public GameObject cameraObj;
    public float STARTTIME = 9.99f;
    public bool isPlaying;
    public int level = 1;
    public int score = 0;
    public float time;
    public float difficulty = 0f;
    public Highscores highscores;

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
        ship = PlayerShip.instance;
        Startup();
        string highscoresPath = Path.Combine(Application.persistentDataPath, "highscores.xml");
        Debug.Log(highscoresPath);
        if (!File.Exists(highscoresPath))
        {
            FileStream stream = File.Create(highscoresPath);
            stream.Close();
            File.WriteAllText(highscoresPath, File.ReadAllText(Path.Combine(Application.dataPath, "Files/highscores.xml")));
        }
        highscores = Highscores.Load(highscoresPath);
    }
	
	void Update ()
    {
        if (isPlaying)
        {
            time -= Time.deltaTime;
            if (time < 0f) time = 0f;
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
