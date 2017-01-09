using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GameManager.instance.GetHighscore() > 20) Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.score > 10) Destroy(gameObject);
	}
}
