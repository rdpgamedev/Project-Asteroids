using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    private Vector3 firstPosition;
    private Quaternion firstRotation;
	// Use this for initialization
	void Start () {
        firstPosition = transform.position;
        firstRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void Restart ()
    {
        transform.position = firstPosition;
        transform.rotation = firstRotation;
        GetComponent<MoveTo>().enabled = true;
        GetComponent<LookAt>().enabled = true;
    }
}
