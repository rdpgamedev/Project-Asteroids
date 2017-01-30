using UnityEngine;
using System.Collections;

public class ConstrainVelocity : MonoBehaviour {
    public float MAXVELOCITY = 40f;

    private Rigidbody rigidBody;
    private Vector3 frozenVelocity;
    private bool frozen;

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
        if ((transform.position - PlayerShip.instance.transform.position).magnitude > 2400f && !frozen)
        {
            frozen = true;
            frozenVelocity = rigidBody.velocity;
            rigidBody.velocity = new Vector3();
        }
        else if ((transform.position - PlayerShip.instance.transform.position).magnitude <= 2400f && frozen)
        {
            frozen = false;
            rigidBody.velocity = frozenVelocity;
        }
	}
}
