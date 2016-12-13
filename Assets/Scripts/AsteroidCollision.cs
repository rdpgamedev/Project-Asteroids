using UnityEngine;
using System.Collections;

public class AsteroidCollision : MonoBehaviour {
    public int colliders;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.name.Contains("Asteroid"))
        {
            //Destroy(collision.gameObject);
        }
    }

    void OnTriggerEnter (Collider collider)
    {
        ++colliders;
        GetComponent<MeshCollider>().enabled = true;
    }

    void OnTriggerExit ()
    {
        --colliders;
        if (colliders == 0) GetComponent<MeshCollider>().enabled = false;
    }
}
