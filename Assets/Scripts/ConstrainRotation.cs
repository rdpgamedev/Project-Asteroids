using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrainRotation : MonoBehaviour {
    public float MAXROTATION;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion rotation = GetComponent<Rigidbody>().rotation;
        Vector3 rotationAngles = rotation.eulerAngles;
        float rotationMag = rotationAngles.magnitude;
        if (rotationMag > MAXROTATION)
        {
            rotationAngles = rotationAngles.normalized * MAXROTATION;
            Quaternion newRotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            newRotation.eulerAngles = rotationAngles;
            GetComponent<Rigidbody>().rotation = newRotation;
        }
	}
}
