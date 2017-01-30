using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSafeZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        string objName = other.gameObject.name;
        if (objName.Contains("Asteroid") || objName.Contains("Landmark"))
        {
            Destroy(other.gameObject);
        }
    }
}
