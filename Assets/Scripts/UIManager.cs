using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject MENUUI;
    public GameObject GAMEUI;
    public GameObject PAUSEUI;
    public GameObject HIGHSCOREUI;
    public GameObject SCORESUI;
    public GameObject OPTIONSUI;
    public GameObject CREDITSUI;

    public enum UIType { MENU, GAME, PAUSE, HIGHSCORE, SCORES, OPTIONS, CREDITS};

    private UIType activeUI;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        MENUUI.SetActive(true);
        activeUI = UIType.MENU;
        ActivateUI(activeUI);
    }

    void Update()
    {

    }

    public void ActivateUI(UIType ui)
    {
        DeactivateUI(activeUI);
        switch (ui)
        {
            case UIType.MENU:
                MENUUI.SetActive(true);
                activeUI = UIType.MENU;
                MENUUI.transform.Find("Buttons").Find("Play Button").GetComponent<Button>().Select();
                break;
            case UIType.GAME:
                GAMEUI.SetActive(true);
                activeUI = UIType.GAME;
                break;
            case UIType.PAUSE:
                PAUSEUI.SetActive(true);
                activeUI = UIType.PAUSE;
                Text scoreText = GAMEUI.transform.Find("Score_Text").GetComponent<Text>();
                Text pauseScoreText = PAUSEUI.transform.Find("Score_Text").GetComponent<Text>();
                pauseScoreText.text = scoreText.text;
                if (PlayerShip.instance.isDead)
                {
                    PAUSEUI.transform.Find("Buttons").Find("Restart Button").GetComponent<Button>().Select();
                    PAUSEUI.transform.Find("Buttons").Find("Resume Button").gameObject.SetActive(false);
                }
                else
                {
                    PAUSEUI.transform.Find("Buttons").Find("Resume Button").GetComponent<Button>().Select();
                }

                break;
            case UIType.HIGHSCORE:
                HIGHSCOREUI.SetActive(true);
                activeUI = UIType.HIGHSCORE;
                HIGHSCOREUI.transform.Find("Score Panel").Find("InputField").GetComponent<InputField>().Select();
                break;
            case UIType.SCORES:
                SCORESUI.SetActive(true);
                activeUI = UIType.SCORES;
                SCORESUI.transform.Find("Menu Button").GetComponent<Button>().Select();
                break;
            case UIType.OPTIONS:
                OPTIONSUI.SetActive(true);
                activeUI = UIType.OPTIONS;
                OPTIONSUI.transform.Find("Options Area").Find("Options").Find("Simplified Controls").GetComponent<Toggle>().Select();
                break;
            case UIType.CREDITS:
                CREDITSUI.SetActive(true);
                activeUI = UIType.CREDITS;
                break;
            default:
                break;
        }
    }

    private void DeactivateUI(UIType ui)
    {
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(null);
        switch (ui)
        {
            case UIType.MENU:
                MENUUI.SetActive(false);
                break;
            case UIType.GAME:
                GAMEUI.SetActive(false);
                break;
            case UIType.PAUSE:
                PAUSEUI.transform.Find("Buttons").Find("Resume Button").gameObject.SetActive(true);
                PAUSEUI.SetActive(false);
                break;
            case UIType.HIGHSCORE:
                HIGHSCOREUI.SetActive(false);
                break;
            case UIType.SCORES:
                SCORESUI.SetActive(false);
                break;
            case UIType.OPTIONS:
                //Save Preferences
                int simplified = 0;
                if (PlayerController.instance.useSimplifiedControls) simplified = 1;
                PlayerPrefs.SetInt("Simplified Controls", simplified);
                int invert = 0;
                if (PlayerController.instance.invertVertical) invert = 1;
                PlayerPrefs.SetInt("Invert Vertical Controls", invert);
                GameObject musicSlider = OPTIONSUI.transform.Find("Options Area").Find("Options").Find("Music Volume").gameObject;
                float musicVolume = musicSlider.GetComponent<Slider>().value;
                PlayerPrefs.SetFloat("Music Volume", musicVolume);
                GameObject effectsSlider = OPTIONSUI.transform.Find("Options Area").Find("Options").Find("Effects Volume").gameObject;
                float effectsVolume = effectsSlider.GetComponent<Slider>().value;
                PlayerPrefs.SetFloat("Effects Volume", effectsVolume);
                OPTIONSUI.SetActive(false);
                break;
            case UIType.CREDITS:
                CREDITSUI.SetActive(false);
                break;
            default:
                break;
        }
    }
}
