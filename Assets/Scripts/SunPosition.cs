using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunPosition : MonoBehaviour {
    public GameObject cameraObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forward = transform.forward.normalized;
        transform.position = cameraObj.transform.position - forward * 4800f;
	}
}
