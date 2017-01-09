using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    private Vector3 firstPosition;
    private Quaternion firstRotation;
    private Vector3 originalGravity;
	// Use this for initialization
	void Start () {
        firstPosition = transform.position;
        firstRotation = transform.rotation;
        originalGravity = Physics.gravity;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newGravity = -transform.up * originalGravity.magnitude;
        Physics.gravity = newGravity;
	}
    
    public void Restart ()
    {
        transform.position = firstPosition;
        transform.rotation = firstRotation;
        GetComponent<MoveTo>().enabled = true;
        GetComponent<LookAt>().enabled = true;
    }
}
