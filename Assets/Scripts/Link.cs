using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Link : MonoBehaviour
{
    public string url;
    public Color normalColor;
    public Color highlightedColor;

    private Text text;

    public void HighlightColor()
    {
        text.color = highlightedColor;
    }

    public void NormalColor()
    {
        text.color = normalColor;
    }

    public void OpenUrl()
    {
        Application.OpenURL(url);
    }

    private void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    private void OnMouseEnter()
    {
        Debug.Log("mouse entered");
    }
}
