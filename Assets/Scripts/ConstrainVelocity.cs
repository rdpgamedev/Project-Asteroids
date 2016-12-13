using UnityEngine;
using System.Collections;

public class ConstrainVelocity : MonoBehaviour {
    public float MAXVELOCITY = 40f;

    private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 velocity = rigidBody.velocity;
        if (velocity.magnitude > MAXVELOCITY)
        {
            velocity.Normalize();
            velocity *= MAXVELOCITY;
            rigidBody.velocity = velocity;
        }
	}
}
