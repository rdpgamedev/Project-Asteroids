using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject GAMEUI;
    public GameObject MENUUI;

    public enum UIType { GAME, MENU };

    private UIType activeUI;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GAMEUI.SetActive(true);
        activeUI = UIType.GAME;
    }

    void Update()
    {

    }

    public void ActivateUI(UIType ui)
    {
        DeactivateUI(activeUI);
        switch (ui)
        {
            case UIType.GAME:
                GAMEUI.SetActive(true);
                break;
            case UIType.MENU:
                MENUUI.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void DeactivateUI(UIType ui)
    {
        switch (ui)
        {
            case UIType.GAME:
                GAMEUI.SetActive(false);
                break;
            case UIType.MENU:
                MENUUI.SetActive(false);
                break;
            default:
                break;
        }
    }
}
