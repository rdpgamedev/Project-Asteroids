using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreInitials : MonoBehaviour {
    InputField initialsField;
	// Use this for initialization
	void Start () {
        initialsField = GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SubmitScore ()
    {
        GameManager.instance.AddScore(initialsField.text);
        UIManager.instance.ActivateUI(UIManager.UIType.PAUSE);
    }
}
